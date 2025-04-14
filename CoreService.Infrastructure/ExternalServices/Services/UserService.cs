using System.Net;
using System.Text.Json;
using Common.Exceptions;
using CoreService.Contracts.Interfaces;
using CoreService.Infrastructure.ExternalServices.ExternalDtos;

namespace CoreService.Infrastructure.ExternalServices.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(Guid userId)
        {
            var client = _httpClientFactory.CreateClient("UserServiceClient");

            var requestUri = $"/api/users/{userId}";

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