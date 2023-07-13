using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Data;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Messages.FindAsync(id);
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            var query = _context.Messages
                .OrderByDescending(x => x.MessageSent)
                .AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.RecipientUserName == messageParams.Username),
                "Outbox" => query.Where(x => x.SenderUserName == messageParams.Username),
                _ => query.Where(x => x.RecipientUserName == messageParams.Username && x.DateRead == null)
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
                    x => x.Recipient.UserName == currentUserName && x.Sender.UserName == recipientUserName ||
                    x.Recipient.UserName == recipientUserName && x.Sender.UserName == currentUserName
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
