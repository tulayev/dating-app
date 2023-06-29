using API.Helpers;
using Models.DTOs;
using Models;

namespace API.Repositories.Repository
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams);
    }
}
