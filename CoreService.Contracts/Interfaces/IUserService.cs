using CoreService.Infrastructure.ExternalServices.ExternalDtos;

namespace CoreService.Contracts.Interfaces;

public interface IUserService
{
    Task<UserInfoDto?> GetUserInfoAsync(Guid userId);
}