using API.Helpers;
using Models.DTOs;
using Models;

namespace API.Repositories.Repository
{
    public interface IUserRepository
    {
        Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
        
        Task<MemberDto> GetMemberAsync(string userName);
        
        Task<IEnumerable<AppUser>> GetUsersAsync();
        
        Task<AppUser> GetUserByIdAsync(int id);
        
        Task<AppUser> GetUserByUserNameAsync(string userName);
        
        Task<string> GetUserGender(string userName);

        void Update(AppUser user);
    }
}
