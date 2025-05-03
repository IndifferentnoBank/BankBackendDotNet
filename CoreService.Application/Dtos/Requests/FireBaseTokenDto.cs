using System.ComponentModel.DataAnnotations;
using Common.Helpers;

namespace CoreService.Application.Dtos.Requests;

public class FireBaseTokenDto
{
    [Required]
    public string Token { get; set; }
    
    [Required]
    public Roles Service { get; set; }
}