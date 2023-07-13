using API.Helpers;
using Models;
using Models.DTOs.Like;

namespace API.Repositories.Repository.Like
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int sourceUserId, int likedUserId);

        Task<AppUser> GetUserWithLikes(int userId);

        Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams);
    }
}
