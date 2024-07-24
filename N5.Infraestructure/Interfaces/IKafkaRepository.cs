namespace N5.Infraestructure.Interfaces;
public interface IKafkaRepository
{
    Task SendMessageToTopic(string topic, string message);
}
