# PlcKafkaLibrary

## Introduction

PlcKafkaLibrary is a library that enables communication between ASP.NET Application and Apache Kafka.

## Installation

You can install PlcKafkaLibrary via NuGet Package Manager by running the following command in the Package Manager Console:

```bash
Install-Package PlcKafkaLibrary
```

Or use the following command in .NET CLI:

```bash
dotnet add package PlcKafkaLibrary
```

## Usage

Here's an example of how to use PlcKafkaLibrary to send and receive data between ASP.NET Application and Kafka:

#### appsettings.json
```json
{
  "Kafka": {
    "BootstrapServers": [
      "localhost:9092"
    ],
    "SaslMechanism": "Plain",
    "SecurityProtocol": "SaslPlaintext",
    "SaslUsername": "username",
    "SaslPassword": "password",
    "AdminClient": {
      "CreateTopic": true,
      "LoggingMetadata": [
        "Broker",
        "Topic"
      ]
    },
    "Producer": {
      "Acks": "All",
      "EnableIdempotence": true,
      "MaxInFlight": 5,
      "RetryBackoffMs": 100,
      "MessageSendMaxRetries": 2147483647,
      "DeliveryTimeoutMs": 120000,
      "CompressionType": "Gzip"
    },
    "Consumer": {
      "GroupId": "Your Consumer Group Id",
      "Timeout": 1000,
      "AutoOffsetReset": "Earliest",
      "EnableAutoOffsetStore": false
    },
    "Topic": {
      "User": {
        "Name": "user-topic",
        "NumPartitions": 3,
        "ReplicationFactor": 1
      },
      "Company": {
        "Name": "company-topic",
        "NumPartitions": 3,
        "ReplicationFactor": 1
      }
    }
  }
}
```

#### Producer
```csharp
using PlcKafkaLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafkaConnection(builder.Configuration);
builder.Services.AddKafkaProducer<string, object>();
```

```csharp
using PlcKafkaLibrary.Producer;

public class YourService
{
    private readonly IKafkaMessageBus<string, object> _kafkaMessageBus;

    public YourService(IKafkaMessageBus<string, object> kafkaMessageBus)
    {
        _kafkaMessageBus = kafkaMessageBus;
    }

    public async Task SendUserMessage(User user)
    {
        string topic = "User";
        string key = Guid.NewGuid().ToString();
        Dictionary<string, byte[]> headers = new Dictionary<string, byte[]>();
        headers.Add("header-label", Encoding.UTF8.GetBytes("header-value"));
        
        await _kafkaMessageBus.PublishAsync(topic, key, user, headers);
    }
```

#### Consumer
```csharp
using PlcKafkaLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafkaConnection(builder.Configuration);
builder.Services.AddKafkaConsumer<string, User, UserEventHandler>();
builder.Services.AddKafkaConsumer<string, Company, CompanyEventHandler>();
```

```csharp
using PlcKafkaLibrary.Consumer;

public class UserEventHandler : IKafkaConsumerHandler<string, User>
{
    public string Topic => "User";

    public async Task HandleAsync(KafkaConsumeResult<string, User> result)
    {
        string topic = result.Topic;
        string key = result.Key;
        User user = result.Value;
        Dictionary<string, byte[]> headers = result.Headers;
        **... your logic ...**
        await Task.CompletedTask;
    }
}
```

## Contribution

Contributions are welcome! If you'd like to contribute to PlcKafkaLibrary, please create an issue or send a pull request on our GitHub repository.

## License

PlcKafkaLibrary is distributed under the [MIT License](LICENSE). See the LICENSE file for more information.

## Contact

If you have any questions or suggestions, feel free to contact me via email: phuoc.truong.1008@gmail.com

--- 

The library is developed by phuocleoceo - Copyright © 2024
