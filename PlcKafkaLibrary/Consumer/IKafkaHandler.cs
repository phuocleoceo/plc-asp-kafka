namespace PlcKafkaLibrary.Consumer;

public interface IKafkaHandler<TK, TV>
{
    Task HandleAsync(TK key, TV value);
}
