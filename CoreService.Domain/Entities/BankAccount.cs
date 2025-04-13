using System.ComponentModel.DataAnnotations;
using CoreService.Domain.Enums;

namespace CoreService.Domain.Entities;

public class BankAccount(Guid userId, string name, string accountNumber, Currency currency)
{
    [Key] 
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required] 
    public string Name { get; init; } = name;

    [Required] 
    public string AccountNumber { get; init; } = accountNumber;

    public decimal Balance { get; set; } = decimal.Zero;

    [Required] 
    public Currency Currency { get; init; } = currency;

    public bool isClosed { get; set; } = false;

    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;

    [Required] 
    public Guid UserId { get; init; } = userId;

    public ICollection<Transaction> Transactions = new List<Transaction>();
}