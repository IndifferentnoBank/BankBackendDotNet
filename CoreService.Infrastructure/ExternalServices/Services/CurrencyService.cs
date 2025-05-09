using System.Text.Json;
using Common.Configurations;
using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using CoreService.Domain.Enums;
using Microsoft.Extensions.Options;


namespace CoreService.Infrastructure.ExternalServices.Services;

public class CurrencyService : ICurrencyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClientConfig _config;


    public CurrencyService(IHttpClientFactory httpClientFactory, IOptions<HttpClientsConfig> config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config.Value.CurrencyServiceClient;
    }

    public async Task<double> ConvertCurrency(double amount, Currency fromCurrency, Currency toCurrency)
    {
        var exchangeRate = await GetExchangeRate(fromCurrency, toCurrency);
        return amount * exchangeRate;
    }

    public async Task<double> GetExchangeRate(Currency fromCurrency, Currency toCurrency)
    {
        var client = _httpClientFactory.CreateClient("CurrencyServiceClient");

        var currencies = $"{toCurrency}&base_currency={fromCurrency}";
        var uri = $"latest?apikey={_config.ApiKey}&currencies={currencies}";

        var response = await client.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var currencyData = JsonSerializer.Deserialize<CurrencyDto>(content, options);

        if (currencyData?.Data == null || !currencyData.Data.TryGetValue(toCurrency, out var toRate))
        {
            throw new Exception($"Failed to retrieve exchange rate for {toCurrency}.");
        }

        return toRate;
    }
}