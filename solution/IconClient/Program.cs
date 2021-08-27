using EasyNetQ;
using Grpc.Net.Client;
using IconServer;
using Messages;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IconClient
{
    class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        public static string AMQP => config.GetConnectionString("AMQP");
        public static string IconServerAddress => config.GetConnectionString("IconServerAddress");

        const string SUBSCRIBER_ID = "IconClient";

        private static Iconer.IconerClient grpcClient;

        static async Task Main(string[] args)
        {
            // Connect to gRPC
            using var channel = GrpcChannel.ForAddress(IconServerAddress);
            grpcClient = new Iconer.IconerClient(channel);

            // Connect to EasyNetQ
            var bus = RabbitHutch.CreateBus(AMQP);
            await bus.PubSub.SubscribeAsync<WeatherForecastMessage>(SUBSCRIBER_ID, GetIconByWeather);

            Console.WriteLine("Ready.");
            Console.Read();
        }

        private static void GetIconByWeather(WeatherForecastMessage message)
        {
            Console.WriteLine(message.WeatherSummary);

            var reply = grpcClient.GetIcon(new IconRequest { WeatherSummary = message.WeatherSummary });

            Console.WriteLine(reply.IconUrl);
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
