using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace WebSender
{
    public class EventConsumer : IConsumer<ValueEntered>
    {
        private readonly ILogger<EventConsumer> _logger;

        public EventConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;
        }
        
        public Task Consume(ConsumeContext<ValueEntered> context)
        {
            _logger.LogInformation($"value : {context.Message.Value}");
            
            return Task.CompletedTask;
        }
    }
}