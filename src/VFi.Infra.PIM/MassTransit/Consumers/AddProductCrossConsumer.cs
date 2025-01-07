using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks; 
using VFi.Domain.PIM.Events;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Domain.PIM.QueryModels;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace VFi.Infra.ACC.MassTransit.Consumers;

public class AddProductCrossConsumer : IConsumer<AddProductCrossQueueEvent>
{
    private readonly ILogger<AddProductCrossQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;
    public AddProductCrossConsumer(ILogger<AddProductCrossQueueEvent> logger, IServiceProvider serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }


    public async Task Consume(ConsumeContext<AddProductCrossQueueEvent> context)
    {
        var msg = context.Message;
        _logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));

        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        var db = _serviceFactory.GetService(typeof(SqlCoreContext));
        var store = _serviceFactory.GetService(typeof(IPIMContextProcedures));
        var repository = _serviceFactory.GetService(typeof(IProductRepository)) as IProductRepository;

        var item = new ProductSimple()
        {
            Id = msg.Id,
            Code = msg.Code,
            ProductType = msg.ProductType,
            Condition = msg.Condition,
            UnitType = msg.UnitType,
            UnitCode = msg.UnitCode,
            Name = msg.Name,
            SourceLink = msg.SourceLink,
            SourceCode = msg.SourceCode,
            ShortDescription = msg.ShortDescription,
            FullDescription = msg.FullDescription,
            LimitedToStores = msg.LimitedToStores,
            Channel = msg.Channel,
            Channel_Category = msg.Channel_Category,
            Origin = msg.Origin,
            Brand = msg.Brand,
            Manufacturer = msg.Manufacturer,
            ManufacturerNumber = msg.ManufacturerNumber,
            Image = msg.Image,
            Images = msg.Images,
            Gtin = msg.Gtin,
            ProductCost = msg.ProductCost,
            CurrencyCost = msg.CurrencyCost,
            Price = msg.Price,
            Currency = msg.Currency,
            IsTaxExempt = msg.IsTaxExempt,
            Tax = msg.Tax,
            OrderMinimumQuantity = msg.OrderMinimumQuantity,
            OrderMaximumQuantity = msg.OrderMaximumQuantity,
            IsShipEnabled = msg.IsShipEnabled,
            IsFreeShipping = msg.IsFreeShipping,
            ProductTag = msg.ProductTag
        };
        var _ = await repository.AddProductSimple(item);


    }
}


