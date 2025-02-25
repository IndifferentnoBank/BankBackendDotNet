using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreService.Domain.Enums;

namespace CoreService.Domain.Entities;

public class Transaction(TransactionType transactionType, double amount, string? comment, BankAccount bankAccount)
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    public DateTime Date { get; init; } = DateTime.Now;

    [Required]
    [Range(0.01, double.MaxValue)]
    public double Amount { get; init; } = amount;

    public string? Comment { get; init; } = comment;
    
    [Required]
    public TransactionType Type { get; init; } = transactionType;
    
    [ForeignKey("BankAccount")]
    public Guid BankAccountId { get; init; } = bankAccount.Id;

    public BankAccount BankAccount { get; init; } = bankAccount;
}