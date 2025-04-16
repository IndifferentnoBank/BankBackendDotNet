using Microsoft.AspNetCore.Authorization;

namespace UserService.Presentation.Authorization;

public class AuthorizationRequirements : IAuthorizationRequirement
{
    public AuthorizationRequirements()
    {
    }
}