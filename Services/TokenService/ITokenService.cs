using Models;

namespace Services.TokenService
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
