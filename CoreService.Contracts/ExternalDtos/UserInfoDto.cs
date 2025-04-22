namespace CoreService.Contracts.ExternalDtos;

public class UserInfoDto
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string FullName { get; set; }

    public string Passport { get; set; }

    public bool IsLocked { get; set; }

    public List<string> Role { get; set; }
}