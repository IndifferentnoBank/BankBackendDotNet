namespace CoreService.Contracts.ExternalDtos;

public class ShortenUserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}