using Common.Configurations;

namespace CoreService.Infrastructure.ExternalServices;

public class HttpClientsConfig
{
    public UserServiceClientConfig UserServiceClient { get; set; }
    public HttpClientConfig CurrencyServiceClient { get; set; }
}

public class UserServiceClientConfig
{
    public string BaseUrl { get; set; }
    public string UserEndpoint { get; set; }
    public string PhoneEndpoint { get; set; }
}