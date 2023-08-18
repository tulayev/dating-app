using API.Helpers;
using Models;
using Models.DTOs.Message;

namespace API.Repositories.Repository.Message
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);

        void RemoveConnection(Connection connection);

        Task<Connection> GetConnection(string connectiondId);

        Task<Group> GetMessageGroup(string groupName);

        Task<Group> GetGroupForConnection(string connectionId);

        void AddMessage(Models.Message message);

        void DeleteMessage(Models.Message message);

        Task<Models.Message> GetMessage(int id);

        Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams);

        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientUserName);
    }
}
