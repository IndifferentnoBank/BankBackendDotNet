using CoreService.Infrastructure.ExternalServices.ExternalDtos;

namespace CoreService.Infrastructure.ExternalServices.UserService;

public interface IUserService
{
    Task<UserInfoDto?> GetUserInfoAsync(Guid userId);
}