using System.Net;
using System.Text.Json;
using Common.Configurations;
using Common.Exceptions;
using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using Microsoft.Extensions.Options;

namespace CoreService.Infrastructure.ExternalServices.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClientConfig _userServiceClientConfig;


        public UserService(IHttpClientFactory httpClientFactory, IOptions<HttpClientsConfig> config )
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _userServiceClientConfig = config.Value.UserServiceClient;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("UserServiceClient");

            var requestUri = $"{_userServiceClientConfig.EndpointName}{userId}";

            var response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserInfoDto>(content, options);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFound("Provided user does not exist");
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }
}