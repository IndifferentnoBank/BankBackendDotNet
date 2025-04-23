namespace CoreService.Infrastructure.ExternalServices;

public class HttpClientConfig
{
    public string BaseUrl { get; set; }
    public string ApiKey { get; set; }
    public string EndpointName { get; set; }
}

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