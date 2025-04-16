using System.ComponentModel.DataAnnotations;
using CoreService.Domain.Enums;

namespace CoreService.Application.Dtos.Requests;

public class CreateBankAccountDto
{
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public Currency Currency { get; set; }
}