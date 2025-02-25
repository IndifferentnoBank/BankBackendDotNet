using System.ComponentModel.DataAnnotations;

namespace CoreService.Application.Dtos.Requests;

public class CreateBankAccountDto
{
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public string Name { get; set; }
}