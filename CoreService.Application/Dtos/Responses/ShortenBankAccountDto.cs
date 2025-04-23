using System.ComponentModel.DataAnnotations;
using CoreService.Contracts.ExternalDtos;
using CoreService.Domain.Enums;

namespace CoreService.Application.Dtos.Responses;

public class ShortenBankAccountDto
{
    [Required] 
    public Guid Id { get; set; }

    [Required] 
    public string Name { get; set; }

    [Required] 
    public string AccountNumber { get; set; }
    
    [Required]
    public Currency Currency { get; set; }
    
    [Required]
    public ShortenUserDto User { get; set; }
}