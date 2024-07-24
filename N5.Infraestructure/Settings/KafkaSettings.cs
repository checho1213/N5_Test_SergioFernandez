namespace N5.Infraestructure.Settings;
public class KafkaSettings
{
    public TopicsSettings Topics { get; set; }
    public string BrokerList { get; set; }
}

public class TopicsSettings
{
    public string PermissionEvent { get; set; }
}
