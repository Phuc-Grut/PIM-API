
using VFi.Application.PIM.Events;
using VFi.Domain.PIM.Interfaces;
using MassTransit.Internals;
using MediatR;
namespace VFi.Application.PIM.Events.Handler;

public class PublishProductTopicItemHandler : INotificationHandler<PublishProductTopicItemEvent>
{

    private readonly IEventRepository eventRepository;
    /// <summary>
    /// /protected readonly IContextUser context;
    /// </summary>
    /// <param name="eventRepository"></param>
    public PublishProductTopicItemHandler(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;
    }

    public async Task Handle(PublishProductTopicItemEvent notification, CancellationToken cancellationToken)
    {

        var message = new VFi.Domain.PIM.Events.PublishProductTopicQueueEvent(); 
        message.Id = notification.Id; 
        message.AggregateId = notification.AggregateId; 
        message.Tenant = notification.Tenant;
        message.Data = notification.Data;
        message.Data_Zone = notification.Data_Zone;
        var result = await eventRepository.PublishProductTopicItem(message);

    }
}
