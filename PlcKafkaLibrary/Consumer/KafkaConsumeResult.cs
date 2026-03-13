using Confluent.Kafka;

namespace PlcKafkaLibrary.Consumer;

public class KafkaConsumeResult<TKey, TValue>(ConsumeResult<TKey, TValue> consumeResult)
{
    public string Topic { get; set; } = consumeResult.Topic;
    public TKey Key { get; set; } = consumeResult.Message.Key;
    public TValue Value { get; set; } = consumeResult.Message.Value;
    public int Partition { get; set; } = consumeResult.Partition.Value;
    public long Offset { get; set; } = consumeResult.Offset.Value;
    public int? LeaderEpoch { get; set; } = consumeResult.LeaderEpoch;
    public bool IsPartitionEof { get; set; } = consumeResult.IsPartitionEOF;
    public Dictionary<string, byte[]> Headers { get; set; } =
        consumeResult.Message.Headers.BackingList.ToDictionary(
            header => header.Key,
            header => header.GetValueBytes()
        );
}
