using System.ComponentModel.DataAnnotations;

namespace CoreService.Domain.Entities;

public class FireBaseToken
{
    [Key]
    public string Token { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
}