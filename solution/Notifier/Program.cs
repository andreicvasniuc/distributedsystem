using EasyNetQ;
using Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Notifier
{
    class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        const string SUBSCRIBER_ID = "Notifier";

        static HubConnection hub;

        static async Task Main(string[] args)
        {
            JsonConvert.DefaultSettings = JsonSettings;

            // Connect to EasyNetQ
            var amqp = config.GetConnectionString("AMQP");
            var bus = RabbitHutch.CreateBus(amqp);
            await bus.PubSub.SubscribeAsync<WeatherIconMessage>(SUBSCRIBER_ID, Handler);

            // Connect to SignalR
            var signalHubUrl = config.GetConnectionString("SignalHubUrl");
            hub = new HubConnectionBuilder().WithUrl(signalHubUrl).Build();
            await hub.StartAsync();

            Console.WriteLine("Listening for WeatherIconMessage messages");
            Console.Read();
        }

        private static async Task Handler(WeatherIconMessage message)
        {
            Console.WriteLine($"Got weather summary {message.WeatherSummary} with icon {message.WeatherIconUrl}");

            try
            {
                var json = JsonConvert.SerializeObject(message);
                Console.WriteLine($"Sending JSON to hub: {json}");
                await hub.SendAsync("NotifyWebUsers", "Notifier", json);
                Console.WriteLine($"Sent!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private static JsonSerializerSettings JsonSettings() => new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
        };

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
