using System;
using System.Threading.Tasks;

namespace Consumer;

internal class Program
{
    static Task Main(string[] args)
    {
        Console.WriteLine("Hello!, This is Kafka Consumer Application");
        var consumeMessage = new ConsumeMessage();
        consumeMessage.ReadMessage();
        return Task.CompletedTask;
    }
}
