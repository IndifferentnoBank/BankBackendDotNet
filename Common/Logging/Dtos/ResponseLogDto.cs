namespace Common.Logging.Dtos;

public class ResponseLogDto
{
    public required string ServiceName { get; set; }
    public required string EventType { get; set; }
    public required string TraceId { get; set; }
    public required string SpanId { get; set; }
    public int DurationMs { get; set; }
    public required string Timestamp { get; set; }
    public required string LogMessage { get; set; }
    public required ResponseData Data { get; set; }
}