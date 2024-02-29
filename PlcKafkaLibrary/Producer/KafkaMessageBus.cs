namespace PlcKafkaLibrary.Producer;

public class KafkaMessageBus<TK, TV> : IKafkaMessageBus<TK, TV>
{
    private readonly KafkaProducer<TK, TV> _producer;

    public KafkaMessageBus(KafkaProducer<TK, TV> producer)
    {
        _producer = producer;
    }

    public async Task PublishAsync(TK key, TV message)
    {
        await _producer.ProduceAsync(key, message);
    }
}
