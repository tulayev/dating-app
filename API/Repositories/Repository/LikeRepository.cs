using API.Helpers;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models;
using Data;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Repositories.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly DataContext _context;

        private readonly IMapper _mapper;
        
        public LikeRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;   
        }

        public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
        {
            return await _context.Likes.FirstOrDefaultAsync(x => x.SourceUserId == sourceUserId && x.LikedUserId == likedUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();

            if (likeParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likeParams.UserId);
                users = likes.Select(like => like.LikedUser);
            }

            if (likeParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likeParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.ProjectTo<LikeDto>(_mapper.ConfigurationProvider);

            return await PagedList<LikeDto>.CreateAsync(likedUsers,
                likeParams.PageNumber, likeParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
