using EasyNetQ;
using Messages;
using System;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        const string AMQP = "host=rabbitmq;username=guest;password=guest";
        static async Task Main(string[] args)
        {
            var bus = RabbitHutch.CreateBus(AMQP);
            var c = 0;
            while (true)
            {
                Console.WriteLine("Enter a message:");
                var message = Console.ReadLine();

                if (c % 2 == 0) {
                    var exampleMessage = new ExampleMessage {
                        MachineName = Environment.MachineName,
                        MessageBody = message,
                        MessageSentDate = DateTimeOffset.UtcNow,
                        MessageNumber = c
                    };
                    await bus.PubSub.PublishAsync<ExampleMessage>(exampleMessage);
                } else
                {
                    await bus.PubSub.PublishAsync<string>($"Message {c}: {message}");
                }                
                Console.WriteLine($"Published message {c}: {message}");
                c++;
                Console.ReadKey(true);
            }
        }
    }
}
