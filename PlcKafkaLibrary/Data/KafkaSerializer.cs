using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace PlcKafkaLibrary.Data;

internal sealed class KafkaSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        if (typeof(T) == typeof(Null))
        {
            return null;
        }

        if (typeof(T) == typeof(Ignore))
        {
            throw new NotSupportedException("Not Supported.");
        }

        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
    }
}
