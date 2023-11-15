using System.Security.Claims;

namespace ChatChit.Helpers
{
    public class TokenHelper
    {
        public static Guid? GetUserIdFromClaims(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return userId;
            }
            return null;
        }

    }
}
