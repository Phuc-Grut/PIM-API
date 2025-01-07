using MassTransit;
using Microsoft.Extensions.Logging;
using VFi.NetDevPack.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Event = VFi.NetDevPack.Messaging.Event;

namespace VFi.Infra.PIM
{
    public class EventConsumer : IConsumer<Event>
    {
        private readonly ILogger<EventConsumer> _logger;

        public EventConsumer(ILogger<EventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Event> context)
        {
             
            _logger.LogInformation("EventConsumer:" + JsonConvert.SerializeObject(context.Message));
            return Task.CompletedTask;
        }
    }
    
}
