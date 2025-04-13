namespace Common.Configurations;

public class KafkaConfiguration
{
    public string Topic { get; set; }
    public string GroupId { get; set; }
    public string BootstrapServers { get; set; }
}