using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks; 
using VFi.Domain.PIM.Events;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Domain.PIM.QueryModels;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using StackExchange.Redis;
using static System.Net.Mime.MediaTypeNames;

namespace VFi.Infra.ACC.MassTransit.Consumers;

public class PublishProductTopicItemConsumer : IConsumer<PublishProductTopicQueueEvent>
{
    private readonly ILogger<PublishProductTopicQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;
    public PublishProductTopicItemConsumer(ILogger<PublishProductTopicQueueEvent> logger, IServiceProvider serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }


    public async Task Consume(ConsumeContext<PublishProductTopicQueueEvent> context)
    {
        var msg = context.Message;
        //_logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));

        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        //var db = _serviceFactory.GetService(typeof(SqlCoreContext)) as SqlCoreContext;
        var store = _serviceFactory.GetService(typeof(IPIMContextProcedures)) as IPIMContextProcedures;
        //var repository = _serviceFactory.GetService(typeof(IProductTopicRepository)) as IProductTopicRepository;
        var repositoryDetail = _serviceFactory.GetService(typeof(IProductTopicDetailRepository)) as IProductTopicDetailRepository;
        var items = await repositoryDetail.GetAllByStatus(2);
        Thread.Sleep(1000);
        foreach (var item in items.OrderBy(x => Guid.NewGuid()))
        {
            Thread.Sleep(50);
            await store.SP_PUBLISH_PRODUCT_TOPIC_DETAILAsync(item.Id, DateTime.Now, null,"");
        }
        



    }
}


