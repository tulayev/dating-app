using API.Extensions;
using API.Repositories;
using API.SignalR.Presence;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.DTOs.Message;

namespace API.SignalR.Message
{
    public class MessageHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        
        private readonly IHubContext<PresenceHub> _presenceHub;
        
        private readonly PresenceTracker _presenceTracker;

        public MessageHub(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<PresenceHub> presenceHub, PresenceTracker presenceTracker)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _presenceHub = presenceHub;
            _presenceTracker = presenceTracker;
        }

        private string GetGroupName(string caller, string other)
        {
            bool stringCompare = string.CompareOrdinal(caller, other) < 0;
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUserName());

            if (group == null ) 
            {
                group = new Group(groupName);
                _unitOfWork.MessageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _unitOfWork.Complete())
                return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _unitOfWork.MessageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId ==  Context.ConnectionId);

            _unitOfWork.MessageRepository.RemoveConnection(connection);
            
            if (await _unitOfWork.Complete())
                return group;

            throw new HubException("Failed to remove hub");
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            var username = Context.User.GetUserName();

            if (username == createMessageDto.RecipientUserName.ToLower())
                throw new HubException("You cannot send messages to yourself!");

            var sender = await _unitOfWork.UserRepository.GetUserByUserNameAsync(username);
            var recipient = await _unitOfWork.UserRepository.GetUserByUserNameAsync(createMessageDto.RecipientUserName);

            if (recipient == null)
                throw new HubException("User not found!");

            var message = new Models.Message
            {
                SenderId = sender.Id,
                RecipientId = recipient.Id,
                SenderUserName = sender.UserName,
                RecipientUserName = recipient.UserName,
                Content = createMessageDto.Content
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _unitOfWork.MessageRepository.GetMessageGroup(groupName);

            if (group.Connections.Any(x => x.UserName == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await _presenceTracker.GetConnectionsForUser(recipient.UserName);

                if (connections != null)
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageReceived", new { userName = sender.UserName, knownAs = sender.KnownAs }); 
            }

            _unitOfWork.MessageRepository.AddMessage(message);

            if (await _unitOfWork.Complete()) 
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var groupName = GetGroupName(Context.User.GetUserName(), otherUser);

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _unitOfWork.MessageRepository.GetMessageThread(Context.User.GetUserName(), otherUser);

            await Clients.Caller.SendAsync("RecieveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();

            await Clients.Group(group.Name).SendAsync("UpdatedGroup", group); 

            await base.OnDisconnectedAsync(exception);
        }
    }
}
