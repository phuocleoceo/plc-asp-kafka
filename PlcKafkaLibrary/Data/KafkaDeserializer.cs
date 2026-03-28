using System.Text;
using Confluent.Kafka;
using PlcKafkaLibrary.Utils;

namespace PlcKafkaLibrary.Data;

internal sealed class KafkaDeserializer<T> : IDeserializer<T>
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (typeof(T) == typeof(Null))
        {
            if (data.Length > 0)
            {
                throw new ArgumentException("The Null data is not null.");
            }
            return default;
        }

        if (typeof(T) == typeof(Ignore))
        {
            return default;
        }

        return JsonUtils.Deserialize<T>(Encoding.UTF8.GetString(data));
    }
}
