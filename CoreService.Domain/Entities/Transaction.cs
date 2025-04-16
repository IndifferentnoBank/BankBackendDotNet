using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreService.Domain.Enums;

namespace CoreService.Domain.Entities;

public class Transaction
{
    public Transaction() { }

    public Transaction(TransactionType transactionType, double amount, string? comment, BankAccount bankAccount, Guid? loanId)
    {
        Id = Guid.NewGuid();
        Date = DateTime.UtcNow;
        Amount = amount;
        Comment = comment;
        Type = transactionType;
        BankAccountId = bankAccount.Id;
        BankAccount = bankAccount;
        Status = TransactionStatus.Processing;
        RelatedLoanId = loanId;
    }
    
    [Key]
    public Guid Id { get; init; }
    
    [Required]
    public DateTime Date { get; init; } 

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
    
    public BankAccount BankAccount { get; init; }
    
    public Guid? RelatedTransactionId { get; set; }
    
    public Guid? RelatedLoanId { get; set; }
}