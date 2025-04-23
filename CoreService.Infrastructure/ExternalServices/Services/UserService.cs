using System.Net;
using System.Text.Json;
using Common.Exceptions;
using CoreService.Contracts.ExternalDtos;
using CoreService.Contracts.Interfaces;
using Microsoft.Extensions.Options;

namespace CoreService.Infrastructure.ExternalServices.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserServiceClientConfig _userServiceClientConfig;


        public UserService(IHttpClientFactory httpClientFactory, IOptions<HttpClientsConfig> config )
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _userServiceClientConfig = config.Value.UserServiceClient;
        }

        public async Task<UserInfoDto> GetUserInfoAsync(Guid userId, string token)
        {
            
            var client = _httpClientFactory.CreateClient("UserServiceClient");
            
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var requestUri = $"{_userServiceClientConfig.UserEndpoint}{userId}";

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

        public async Task<ShortenUserDto> GetUserByPhoneNumberAsync(string phone)
        {
            var client = _httpClientFactory.CreateClient("UserServiceClient");
            
            var requestUri = $"{_userServiceClientConfig.PhoneEndpoint}?phone={phone}";

            var response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ShortenUserDto>(content, options);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFound("Provided user does not exist");
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
        }
    }
}