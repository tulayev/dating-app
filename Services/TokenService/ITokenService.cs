using Models;

namespace Services.TokenService
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
