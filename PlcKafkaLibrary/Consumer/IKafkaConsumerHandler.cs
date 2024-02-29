namespace PlcKafkaLibrary.Consumer;

public interface IKafkaConsumerHandler<TK, TV>
{
    Task HandleAsync(TK key, TV value);
}
