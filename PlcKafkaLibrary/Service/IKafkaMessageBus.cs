namespace PlcKafkaLibrary.Service;

public interface IKafkaMessageBus<TK, TV>
{
    Task PublishAsync(TK key, TV message);
}
