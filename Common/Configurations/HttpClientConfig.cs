namespace Common.Configurations;

public class HttpClientConfig
{
    public string BaseUrl { get; set; }
    public string ApiKey { get; set; }
    public string EndpointName { get; set; }
}

public class HttpClientsConfig
{
    public HttpClientConfig UserServiceClient { get; set; }
    public HttpClientConfig CurrencyServiceClient { get; set; }
}