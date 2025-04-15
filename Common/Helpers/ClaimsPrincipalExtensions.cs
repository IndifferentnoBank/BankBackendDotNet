using System.Security.Claims;

namespace Common.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static UserClaims GetUserClaims(this ClaimsPrincipal user)
    {
        if (user == null)
            throw new UnauthorizedAccessException("User is not authenticated");

        var userIdClaim = user.FindFirst("sub") ?? user.FindFirst("userId");
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID claim not found");

        var userId = Guid.Parse(userIdClaim.Value);

        var roleClaims = user.FindAll("roles").Select(c => c.Value).ToList();

        var roles = new List<Roles>();
        foreach (var role in roleClaims)
        {
            if (Enum.TryParse<Roles>(role, ignoreCase: true, out var parsedRole))

                roles.Add(parsedRole);
        }

        return new UserClaims
        {
            UserId = userId,
            Roles = roles
        };
    }
}