
using VFi.Domain.PIM.Events;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace VFi.Infra.PIM.Repository;

public class EventRepository : IEventRepository
{
    private readonly ILogger<EventRepository> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public EventRepository()
    {
    }

    public EventRepository(ILogger<EventRepository> logger, IPublishEndpoint publishEndpoint = null)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> AddProductTopic(AddProductTopicQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> PublishProductTopicItem(PublishProductTopicQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
    public async Task<bool> AddProductCross(AddProductCrossQueueEvent message)
    {
        await _publishEndpoint.Publish(message);
        return true;
    }
}
