using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using CoreService.Domain.Enums;
using MediatR;

namespace CoreService.Application.Features.Queries.GetExchangeRate;

public class GetExchangeRateCommandHandler : IRequestHandler<GetExchangeRateCommand, CurrencyDto>
{
    private readonly ICurrencyService _currencyService;

    public GetExchangeRateCommandHandler(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task<CurrencyDto> Handle(GetExchangeRateCommand request, CancellationToken cancellationToken)
    {
        var usd = await _currencyService.GetExchangeRate(Currency.USD, Currency.RUB);
        var eur = await _currencyService.GetExchangeRate(Currency.EUR, Currency.RUB);
        
        return new CurrencyDto
        {
            Data = new Dictionary<Currency, double>
            {
                {Currency.USD, usd},
                {Currency.EUR, eur}
            }
        };
    }
}