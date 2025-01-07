
using VFi.Application.PIM.Events;
using VFi.Domain.PIM.Interfaces;
using MassTransit.Internals;
using MediatR;
namespace VFi.Application.PIM.Events.Handler;

public class AddProductTopicHandler : INotificationHandler<AddProductTopicEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public AddProductTopicHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(AddProductTopicEvent notification, CancellationToken cancellationToken)
    {

        var message = new VFi.Domain.PIM.Events.AddProductTopicQueueEvent();
        message.Topic = notification.Topic;
        message.Id = notification.Id;


        message.AggregateId = notification.AggregateId;
       // message.AggregateName = notification.CreatedByName;
        message.Tenant = notification.Tenant;
        message.Data = notification.Data;
        message.Data_Zone = notification.Data_Zone;
        var result = await eventRepository.AddProductTopic(message);

    }
}
