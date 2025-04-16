using AutoMapper;
using CoreService.Application.Dtos.Responses;
using CoreService.Domain.Entities;
using CoreService.Infrastructure.SignalR;

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