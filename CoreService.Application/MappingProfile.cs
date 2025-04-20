using AutoMapper;
using CoreService.Application.Dtos.Responses;
using CoreService.Contracts.ExternalDtos;
using CoreService.Domain.Entities;

namespace CoreService.Application;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>();
        CreateMap<BankAccount, BankAccountDto>();
        CreateMap<Transaction, LoanTransactionDto>();
    }
}