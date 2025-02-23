using System.ComponentModel.DataAnnotations;

namespace CoreService.Domain.Entities;

public class BankAccount
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string AccountNumber { get; set; }
    
    [Required]
    public decimal Balance { get; set; }
    
    public bool isClosed { get; set; } = false;
    
    public DateTime Created { get; set; } = DateTime.Now;
    
    ICollection<Transaction> Transactions = new List<Transaction>();
}