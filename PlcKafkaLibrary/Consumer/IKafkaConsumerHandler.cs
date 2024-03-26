namespace PlcKafkaLibrary.Consumer;

public interface IKafkaConsumerHandler<TKey, TValue>
{
    string Topic { get; }

    Task HandleAsync(KafkaConsumeResult<TKey, TValue> result);
}
