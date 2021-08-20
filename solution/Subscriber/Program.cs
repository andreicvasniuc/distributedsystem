using EasyNetQ;
using Messages;
using System;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        const string AMQP = "host=rabbitmq;username=guest;password=guest";
        static async Task Main(string[] args)
        {
            var SUBSCRIBER = $"andrei@{Environment.MachineName}";
            var bus = RabbitHutch.CreateBus(AMQP);
            await bus.PubSub.SubscribeAsync<string>(SUBSCRIBER, s => Console.WriteLine(s));
            await bus.PubSub.SubscribeAsync<ExampleMessage>(SUBSCRIBER, HandleExampleMessage);
            Console.WriteLine("Subscribed to <string> messages.");
            Console.ReadKey(true);
        }

        static void HandleExampleMessage(ExampleMessage message)
        {
            Console.WriteLine(new String('-', 72));
            Console.WriteLine(message.MessageBody);
            Console.WriteLine($"Created at {message.MessageSentDate} on {message.MachineName}");
            Console.WriteLine($"This was message #{message.MessageNumber}");
        }
    }
}