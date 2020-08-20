using System;
using System.Threading;
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

namespace Receiver
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("127.0.0.1", "/", h =>
                {
                    h.Username("cash");
                    h.Password("cash");
                });

                cfg.ReceiveEndpoint("event-listener", e =>
                {
                    e.Consumer<EventConsumer>();
                });
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);

            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(Console.ReadLine, source.Token);
            }
            finally
            {
                await busControl.StopAsync(source.Token);
            }
        }
    }

    class EventConsumer : IConsumer<ValueEntered>
    {
        public Task Consume(ConsumeContext<ValueEntered> context)
        {
            Console.WriteLine("Received Value: {0}", context.Message.Value);

            return Task.CompletedTask;
        }
    }
}