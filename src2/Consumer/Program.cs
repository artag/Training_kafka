using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Consumer;

class Program
{
    private const string topic = "purchases";
    private readonly string[] _args;

    public Program(string[] args)
    {
        _args = args;
    }

    public void Run(CancellationTokenSource cts)
    {
        ValidateArgsOrExit();
        var cfg = BuildConfiguration(_args[0]);

        using var consumer = new ConsumerBuilder<string, string>(cfg).Build();
        consumer.Subscribe(topic);
        try
        {
            while(true)
            {
                var cr = consumer.Consume(cts.Token);
                Console.WriteLine(
                    $"Consumed event from topic {topic}: " +
                    $"key = {cr.Message.Key, -10} " +
                    $"value = {cr.Message.Value}");
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancel operation. Ctrl-C was pressed.");
        }
        finally
        {
            consumer.Close();
        }
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

        configuration["group.id"] = "kafka-dotnet-getting-started";
        configuration["auto.offset.reset"] = "earliest";

        return configuration.AsEnumerable();
    }

    static void Main(string[] args)
    {
        var program = new Program(args);

        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (_, e) =>
        {
            // Prevent the process from terminating.
            e.Cancel = true;
            cts.Cancel();
        };

        program.Run(cts);
    }
}
