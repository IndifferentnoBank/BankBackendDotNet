using UserService.Domain.Entities;

namespace UserService.Application.Dtos.Responses;

public class ShortenUserDto(User user)
{
    public Guid Id { get; init; } = user.Id;
    public string FullName { get; init; } = user.FullName;
    public string PhoneNumber { get; init; } = user.PhoneNumber;
}