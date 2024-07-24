namespace N5.Infraestructure.Repositories;
public class KafkaRepository : IKafkaRepository
{
    private readonly InfraestructureSettings _settings;
    public KafkaRepository(IOptions<InfraestructureSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendMessageToTopic(string topic, string message)
    {
        await KafkaProducerBase.Produce(topic, message, _settings.KafkaSettings.BrokerList);
    }
}
