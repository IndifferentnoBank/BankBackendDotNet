using System.ComponentModel.DataAnnotations;
using CoreService.Domain.Enums;

namespace CoreService.Contracts.ExternalDtos;

public class TransactionDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime Date { get; set; } 

    [Required]
    public double Amount { get; set; }

    public string? Comment { get; set; } 
    
    [Required]
    public TransactionType Type { get; set; } 
    
    [Required]
    public TransactionStatus Status { get; set; }
}