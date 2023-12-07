using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Consumer;

public class ConsumeMessage
{
    public void ReadMessage()
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:29092",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            ClientId = "my-app",
            GroupId = "my-group",
            BrokerAddressFamily = BrokerAddressFamily.V4,
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe("my-topic");

        try
        {
            while (true) {
                var consumeResult = consumer.Consume();
                Console.WriteLine("Message received from {0}: {1}",
                    consumeResult.TopicPartitionOffset, consumeResult.Message.Value);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("The consumer was stopped via cancellation token.");
        }
        finally
        {
            consumer.Close();
        }
        Console.ReadLine();
    }
}
