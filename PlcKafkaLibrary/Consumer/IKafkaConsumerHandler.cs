namespace PlcKafkaLibrary.Consumer;

public interface IKafkaConsumerHandler<TKey, TValue>
{
    Task HandleAsync(TKey key, TValue value);
}
