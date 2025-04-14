using System.ComponentModel.DataAnnotations;
using CoreService.Domain.Enums;

namespace CoreService.Application.Dtos.Requests;

public class CreateTransactionDto
{
    [Required] public TransactionType Type { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public double Amount { get; set; }

    public string? Comment { get; set; }

    public Guid? RelatedLoanId { get; set; }
}