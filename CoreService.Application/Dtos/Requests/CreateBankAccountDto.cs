using System.ComponentModel.DataAnnotations;

namespace CoreService.Application.Dtos.Requests;

public class CreateBankAccountDto
{
    
    [Required]
    public string Name { get; set; }
}