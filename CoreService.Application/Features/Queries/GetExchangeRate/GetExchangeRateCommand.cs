using CoreService.Contracts.ExternalDtos;
using MediatR;

namespace CoreService.Application.Features.Queries.GetExchangeRate;

public record GetExchangeRateCommand() : IRequest<CurrencyDto>;