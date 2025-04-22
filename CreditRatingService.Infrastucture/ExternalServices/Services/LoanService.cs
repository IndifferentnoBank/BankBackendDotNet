using Common.Configurations;
using Common.Exceptions;
using CreditRatingService.Contracts.Dtos;
using CreditRatingService.Contracts.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CreditRatingService.Infrastucture.ExternalServices.Services
{
    public class LoanService : ILoanService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClientConfig _loanServiceClientConfig;

        public LoanService(IHttpClientFactory httpClientFactory, IOptions<HttpClientConfig> config)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _loanServiceClientConfig = config.Value;
        }

        public async Task<LoanDto?> GetLoanByLoanIdAsync(Guid loanId)
        {
            var client = _httpClientFactory.CreateClient("LoanServiceClient");

            var requestUri = $"{_loanServiceClientConfig.EndpointName}{loanId}";

            var response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LoanDto>(content, options);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFound("Provided loan does not exist");
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }

        public async Task<List<LoanDto>> GetLoansByUserIdAsync(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("LoanServiceClient");
            var requestUri = $"{_loanServiceClientConfig.EndpointName}{userId}";

            var response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var content = await response.Content.ReadAsStringAsync();

                var loans = JsonSerializer.Deserialize<List<LoanDto>>(content, options);

                return loans ?? new List<LoanDto>();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFound($"Loans for user with id {userId} not found");
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }
}