using EasyNetQ;
using Messages;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AuditLog
{
    class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        const string SUBSCRIBER_ID = "AuditLog";

        static async Task Main(string[] args)
        {
            var amqp = config.GetConnectionString("AMQP");
            var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<WeatherForecastMessage>(SUBSCRIBER_ID, Handler);
            Console.WriteLine("Listening for WeatherForecastMessage messages");
            Console.Read();
        }

        private static void Handler(WeatherForecastMessage message)
        {
            Console.WriteLine(message.WeatherSummary);
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
