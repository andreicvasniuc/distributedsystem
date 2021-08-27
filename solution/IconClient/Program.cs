using Grpc.Net.Client;
using IconServer;
using System;

namespace IconClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var iconer = new Iconer.IconerClient(channel);
            var reply = iconer.GetIcon(new IconRequest { WeatherSummary = "Hot" });
            Console.WriteLine(reply.IconUrl);
        }
    }
}
