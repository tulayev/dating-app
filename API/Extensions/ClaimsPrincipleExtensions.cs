using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal principal) =>
            principal.FindFirst(ClaimTypes.Name)?.Value;
        
        public static int GetUserId(this ClaimsPrincipal principal) =>
            int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
}
