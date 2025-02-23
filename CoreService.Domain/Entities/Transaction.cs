using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreService.Domain.Entities;

public class Transaction
{
    [Key]
    public Guid Id { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue)]
    public double Amount { get; set; }
    
    public string Comment { get; set; }
    
    [ForeignKey("BankAccount")]
    public Guid BankAccountId { get; set; }
    
    public BankAccount BankAccount { get; set; }
}