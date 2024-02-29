namespace PlcKafkaLibrary.Producer;

public interface IKafkaMessageBus<TK, TV>
{
    Task PublishAsync(TK key, TV message);
}
