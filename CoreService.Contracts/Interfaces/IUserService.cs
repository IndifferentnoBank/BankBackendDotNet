using CoreService.Contracts.ExternalDtos;

namespace CoreService.Contracts.Interfaces;

public interface IUserService
{
    Task<UserInfoDto> GetUserInfoAsync(Guid userId, string token);
    Task<ShortenUserDto> GetUserByPhoneNumberAsync(string phone);
}