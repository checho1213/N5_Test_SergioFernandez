namespace N5.Infraestructure.Base;
public class KafkaProducerBase
{
    #region IKafkaProducer Members
    public static async Task Produce(string topic, string message, string brokerList)
    {
        using (var producer = new ProducerBuilder<string, string>(ConstructConfig(brokerList)).Build())
        {
            await producer.ProduceAsync(topic, new Message<string, string>()
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });
        }
    }
    #endregion

    private static Dictionary<string, string> ConstructConfig(string brokerList) =>
          new Dictionary<string, string>
          {
            { "bootstrap.servers", brokerList }
          };
}
