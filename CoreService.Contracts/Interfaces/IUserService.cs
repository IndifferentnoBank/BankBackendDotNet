using CoreService.Contracts.ExternalDtos;

namespace CoreService.Contracts.Interfaces;

public interface IUserService
{
    Task<UserInfoDto?> GetUserInfoAsync(Guid userId);
}