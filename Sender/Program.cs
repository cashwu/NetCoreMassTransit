using System;
using System.Threading.Tasks;
using EventContracts;
using MassTransit;

namespace EventContracts
{
    public interface ValueEntered
    {
        string Value { get; }
    }
}

namespace Sender
{
    class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                configurator.Host("127.0.0.1", "/", h =>
                {
                    h.Username("cash");
                    h.Password("cash");
                });
            });

            await busControl.StartAsync();

            try
            {
                do
                {
                    var value = await Task.Run(() =>
                    {
                        Console.WriteLine("Enter message (or quit to exit)");
                        Console.Write("> ");

                        return Console.ReadLine();
                    });

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    await busControl.Publish<ValueEntered>(new
                    {
                        Value = value
                    });
                }
                while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}