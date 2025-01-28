using System.Security.Claims;

namespace API.Extentions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal User)
        {
            string username = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                              ?? throw new Exception("Cannot get username from token");

            return username;
        }
    }
}
