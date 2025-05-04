namespace Common.Logging.Dtos;

public class RequestData
{
    public required string HttpMethod { get; set; }
    public required string Url { get; set; }
}