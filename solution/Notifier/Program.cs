using EasyNetQ;
using Messages;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notifier
{
    class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        const string SUBSCRIBER_ID = "Notifier";

        static async Task Main(string[] args)
        {
            var amqp = config.GetConnectionString("AMQP");
            var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<WeatherIconMessage>(SUBSCRIBER_ID, Handler);
            Console.WriteLine("Listening for WeatherIconMessage messages");
            Console.Read();
        }

        private static void Handler(WeatherIconMessage message)
        {
            Console.WriteLine($"Getting weather summary {message.WeatherSummary} with icon {message.WeatherIconUrl}");
        }

        private static IConfigurationRoot ReadConfiguration()
        {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
