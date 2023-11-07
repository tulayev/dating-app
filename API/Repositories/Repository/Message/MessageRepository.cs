using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs.Message;

namespace API.Repositories.Repository.Message
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddGroup(Group group)
        {
            _context.Groups.Add(group); 
        }

        public async Task<Connection> GetConnection(string connectiondId)
        {
            return await _context.Connections.FindAsync(connectiondId);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _context.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _context.Groups 
                .Include(x => x.Connections)
                .Where(x => x.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }

        public void RemoveConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
        }

        public void AddMessage(Models.Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Models.Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Models.Message> GetMessage(int id)
        {
            return await _context.Messages
                .Include(x => x.Sender)
                .Include(x => x.Recipient)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.MessageContainer switch
            {
                MessageContainer.Inbox => query.Where(x => x.RecipientUserName == messageParams.UserName && !x.RecipientDeleted),
                MessageContainer.Outbox => query.Where(x => x.SenderUserName == messageParams.UserName && !x.SenderDeleted),
                _ => query.Where(x => x.RecipientUserName == messageParams.UserName && !x.RecipientDeleted && x.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize); 
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName)
        {
            var messages = await _context.Messages
                .Include(x => x.Sender).ThenInclude(x => x.Photos)
                .Include(x => x.Recipient).ThenInclude(x => x.Photos)
                .Where(
                    x => x.Recipient.UserName == currentUserName && !x.RecipientDeleted && x.Sender.UserName == recipientUserName ||
                    x.Recipient.UserName == recipientUserName && x.Sender.UserName == currentUserName && !x.SenderDeleted
                )
                .OrderBy(x => x.MessageSent)
                .ToListAsync();

            var unreadMessages = messages.Where(x => x.DateRead == null && x.Recipient.UserName == currentUserName)
                .ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages) 
                    message.DateRead = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }
    }
}
