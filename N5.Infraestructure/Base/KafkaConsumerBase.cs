namespace N5.Infraestructure.Base;
public class KafkaConsumerBase<T> where T : class
{
    #region Members
    private readonly IConsumer<string, string> consumer;
    // private readonly ILogger logger;
    #endregion

    #region Constructor
    public KafkaConsumerBase(string brokerList, string consumerGroup, string topics)
    {
        string _brokerList = brokerList ?? throw new ArgumentNullException(nameof(brokerList));
        string _consumerGroup = consumerGroup ?? throw new ArgumentNullException(nameof(consumerGroup));
        string _topics = topics ?? throw new ArgumentNullException(nameof(topics));
        // this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        consumer = new ConsumerBuilder<string, string>(ConstructConfig(_consumerGroup, _brokerList)).Build();
        consumer.Subscribe(_topics.Split(','));
    }
    #endregion

    #region IKafkaConsumer Members
    public IEnumerable<T> Consume(int messageBatch, int timeOut)
    {
        var values = new List<T>();

        do
        {
            try
            {
                var consumeResult = consumer.Consume(TimeSpan.FromSeconds(timeOut));

                if (consumeResult == null)
                    break;
                else
                {
                    if (!consumeResult.IsPartitionEOF)
                    {
                        if (typeof(T) == typeof(string))
                        {
                            values.Add((T)(object)consumeResult.Message.Value);
                        }
                        else
                        {
                            T value = DeserializeT(consumeResult.Message.Value);
                            if (value != null)
                                values.Add(value);
                        }
                        consumer.Commit();
                    }
                }
            }
            catch (ConsumeException ex)
            {
                if (ex.Message.ToUpper().Contains("TIMED OUT") || ex.Message.ToLower().Contains("max.poll.interval.ms"))
                {
                    // this.logger.LogError(ex, $"KafkaConsumerBase-Error consuming messages from kafka topic. {ex.ToString()}");
                    return values;

                }
                else
                    throw;
            }
            catch (TopicPartitionOffsetException ex)
            {
                //this.logger.LogError(ex, $"KafkaConsumerBase-Error consuming messages from kafka topic. {ex.ToString()}");
                return values;
            }
        }
        while (values.Count < messageBatch);

        return values;

    }

    public void Commit()
    {
        consumer.Commit();
    }
    #endregion

    #region Private Members
    private static ConsumerConfig ConstructConfig(string groupId, string brokerList) =>

        new ConsumerConfig
        {

            BootstrapServers = brokerList,
            GroupId = groupId,
            StatisticsIntervalMs = 5000,
            AutoCommitIntervalMs = 10000,
            SessionTimeoutMs = 6000,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            EnablePartitionEof = true
        };

    private T DeserializeT(string message)
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
        catch (Newtonsoft.Json.JsonException ex)
        {
            return null;
        }
    }
    #endregion
}
