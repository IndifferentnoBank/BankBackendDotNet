using System.ComponentModel.DataAnnotations;
using CoreService.Domain.Enums;

namespace CoreService.Application.Dtos.Responses;

public class LoanTransactionDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime Date { get; set; } 

    [Required]
    public double Amount { get; set; }
    
    [Required]
    public TransactionType Type { get; set; } 
    
    [Required]
    public TransactionStatus Status { get; set; }
    
    [Required]
    public Guid RelatedLoanId { get; set; }
}