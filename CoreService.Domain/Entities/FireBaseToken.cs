using System.ComponentModel.DataAnnotations;
using Common.Helpers;

namespace CoreService.Domain.Entities;

public class FireBaseToken
{
    [Key]
    public string Token { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public Roles Service  { get; set; }
}