using Confluent.Kafka;

namespace PlcKafkaLibrary.Consumer;

public class KafkaConsumeResult<TKey, TValue>
{
    public string Topic { get; set; }
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    public int Partition { get; set; }
    public long Offset { get; set; }
    public int? LeaderEpoch { get; set; }
    public bool IsPartitionEof { get; set; }
    public Dictionary<string, byte[]> Headers { get; set; }

    public KafkaConsumeResult(ConsumeResult<TKey, TValue> consumeResult)
    {
        Topic = consumeResult.Topic;
        Key = consumeResult.Message.Key;
        Value = consumeResult.Message.Value;
        Partition = consumeResult.Partition.Value;
        Offset = consumeResult.Offset.Value;
        LeaderEpoch = consumeResult.LeaderEpoch;
        IsPartitionEof = consumeResult.IsPartitionEOF;
        Headers = consumeResult.Message.Headers.BackingList.ToDictionary(
            header => header.Key,
            header => header.GetValueBytes()
        );
    }
}
