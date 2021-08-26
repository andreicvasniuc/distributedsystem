using Grpc.Net.Client;
using GrpcServer;
using System;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var greeter = new Greeter.GreeterClient(channel);
            var reply = greeter.SayHello(new HelloRequest { Name = "Світ", Language = "ua-UA" });
            var reply2 = greeter.SayHello(new HelloRequest { Name = "World" });
            Console.WriteLine(reply.Message);
            Console.WriteLine(reply2.Message);
        }
    }
}
