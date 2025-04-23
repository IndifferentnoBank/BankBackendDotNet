namespace Common.Helpers;

public class UserClaims
{
    public Guid UserId { get; set; }
    public List<Roles> Roles { get; set; } = new List<Roles>();
    
    public string Token { get; set; }
}