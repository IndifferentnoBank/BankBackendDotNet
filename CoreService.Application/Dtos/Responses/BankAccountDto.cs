using System.ComponentModel.DataAnnotations;

namespace CoreService.Application.Dtos.Responses;

public class BankAccountDto
{
    [Required] 
    public Guid Id { get; set; }

    [Required] 
    public string Name { get; set; }

    [Required] 
    public string AccountNumber { get; set; }

    [Required] 
    public decimal Balance { get; set; } 

    [Required] 
    public bool isClosed { get; set; } 

    [Required] 
    public DateTime CreatedDate { get; set; }
}