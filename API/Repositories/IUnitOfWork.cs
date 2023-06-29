using API.Repositories.Repository;

namespace API.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        ILikeRepository LikesRepository { get; }
        
        Task<bool> Complete();
        
        bool HasChanges();
    }
}
