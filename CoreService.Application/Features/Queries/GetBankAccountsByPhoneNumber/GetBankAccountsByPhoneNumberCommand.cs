using CoreService.Application.Dtos.Responses;
using MediatR;

namespace CoreService.Application.Features.Queries.GetBankAccountsByPhoneNumber;

public record GetBankAccountsByPhoneNumberCommand(string PhoneNumber) :IRequest<List<ShortenBankAccountDto>>;