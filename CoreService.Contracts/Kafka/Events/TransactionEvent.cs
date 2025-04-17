using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreService.Domain.Enums;

namespace CoreService.Contracts.Kafka.Events;

public class TransactionEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] 
    public DateTime Date { get; init; } = DateTime.UtcNow;

    [Required]
    [Range(0.01, double.MaxValue)]
    public double Amount { get; init; }

    [Required]
    public Currency Currency { get; set; }

    public string? Comment { get; init; }
    
    [Required]
    public TransactionType Type { get; init; }

    [Required] 
    public TransactionStatus Status { get; set; } = TransactionStatus.Processing;
    
    [ForeignKey("BankAccount")]
    public Guid BankAccountId { get; init; } 
    
    public Guid? RelatedTransactionId { get; set; }
    
    public Guid? RelatedLoanId { get; set; }
}