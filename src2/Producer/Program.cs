using System;
using System.Collections.Generic;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Producer;

class Program
{
    private const string topic = "purchases";
    private const int numMessages = 10;

    private readonly string[] _users =
    {
        "eabara", "jsmith", "sgarcia", "jbernard", "htanaka", "awalther"
    };

    private readonly string[] _items =
    {
        "book", "alarm clock", "t-shirts", "gift card", "batteries"
    };

    private readonly string[] _args;
    private readonly Random _rnd;

    public Program(string[] args)
    {
        _args = args;
        _rnd = new Random();
    }

    public void Run()
    {
        ValidateArgsOrExit();
        var cfg = BuildConfiguration(_args[0]);

        var numProduced = 0;
        using var producer = new ProducerBuilder<string, string>(cfg).Build();

        for (var i = 0; i < numMessages; i++)
        {
            var user = _users[_rnd.Next(_users.Length)];
            var item = _items[_rnd.Next(_items.Length)];

            var message = new Message<string, string>
            {
                Key = user,
                Value = item,
            };

            producer.Produce(
                topic,
                message,
                deliveryReport =>
                {
                    if (deliveryReport.Error.Code != ErrorCode.NoError)
                    {
                        Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                    }
                    else
                    {
                        Console.WriteLine($"Produced event to topic {topic}: key = {user,-10} value = {item}");
                        numProduced += 1;
                    }
                });
        }

        producer.Flush(TimeSpan.FromSeconds(10));
        Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
    }

    private void ValidateArgsOrExit()
    {
        if (_args.Length == 1)
            return;

        Console.WriteLine("Please provide the configuration file path as a command line argument");
        Environment.Exit(1);
    }

    private IEnumerable<KeyValuePair<string, string>> BuildConfiguration(string arg)
    {
        var configuration = new ConfigurationBuilder()
            .AddIniFile(arg)
            .Build();

        return configuration.AsEnumerable();
    }

    static void Main(string[] args)
    {
        var program = new Program(args);
        program.Run();
    }
}
