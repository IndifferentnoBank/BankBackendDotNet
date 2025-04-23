using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Common.Helpers;

public static class JwtHelper
{
    public static UserClaims ExtractUserClaimsFromHeader(HttpContext httpContext)
    {
        var token = GetToken(httpContext);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
        var rolesClaims = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "roles");

        var userClaims = new UserClaims
        {
            UserId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : Guid.Empty,
            Roles = rolesClaims
                .Select(r => Enum.TryParse<Roles>(r.Value, out var parsed) ? parsed : (Roles?)null)
                .Where(r => r.HasValue)
                .Select(r => r!.Value)
                .ToList(),
            Token = token
        };

        return userClaims;
    }

    public static string GetToken(HttpContext httpContext)
    {
        return httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
    }
}