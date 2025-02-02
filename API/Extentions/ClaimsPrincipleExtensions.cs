using System.Security.Claims;

namespace API.Extentions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUserName(this ClaimsPrincipal User)
        {
            string username = User.FindFirstValue(ClaimTypes.Name) 
                              ?? throw new Exception("Cannot get username from token");

            return username;
        }

        public static int GetUserId(this ClaimsPrincipal User)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? throw new Exception("Cannot get user Id from token"));

            return userId;
        }
    }
}
