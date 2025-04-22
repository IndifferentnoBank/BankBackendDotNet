
using Common.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Common.Exceptions;
using CreditRatingService.Contracts.Interfaces;
using CreditRatingService.Contracts.Dtos;

namespace CreditRatingService.Infrastucture.ExternalServices.Services
{
    public class CoreService: ICoreService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClientConfig _coreServiceClientConfig;


        public CoreService(IHttpClientFactory httpClientFactory, IOptions<HttpClientConfig> config)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _coreServiceClientConfig = config.Value;
        }

        public async Task<List<LoanTransactionDto>> GetTransactionByLoanIdAsync(Guid loanId)
        {
            var client = _httpClientFactory.CreateClient("CoreServiceClient");

            var requestUri = $"{_coreServiceClientConfig.EndpointName}{loanId}/transactions";

            var response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var content = await response.Content.ReadAsStringAsync();
                var transactions = JsonSerializer.Deserialize<List<LoanTransactionDto>>(content, options);
                return transactions ?? new List<LoanTransactionDto>();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFound("Provided loan does not exist");
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }
}
