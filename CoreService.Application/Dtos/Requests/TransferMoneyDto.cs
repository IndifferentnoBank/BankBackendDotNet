using System.ComponentModel.DataAnnotations;
using CoreService.Domain.Enums;

namespace CoreService.Application.Dtos.Requests;

public class TransferMoneyDto
{
    [Required]
    public Guid FromBankAccountId { get; set; }
    
    [Required]
    public Guid ToBankAccountId { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public double Amount { get; set; }
    
    [Required]
    public Currency Currency { get; set; }
    
    public string? Comment { get; set; }
    
    
}