using System;
using System.Threading.Tasks;

namespace Producer;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello!, This is my Kafka Producer Application");
        var produceMessage = new ProduceMessage();
        await produceMessage.CreateMessage();
    }
}
