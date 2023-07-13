using API.Repositories.Repository.Like;
using API.Repositories.Repository.Message;
using API.Repositories.Repository.User;

namespace API.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        ILikeRepository LikesRepository { get; }
        
        IMessageRepository MessageRepository { get; }
        
        Task<bool> Complete();
        
        bool HasChanges();
    }
}
