
using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Events;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.Queries;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Domain;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace VFi.Api.PIM.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IContextUser _context;
    private readonly IMediatorHandler _mediator; private readonly CodeSyntaxConfig _codeSyntax;

    public EventController(IContextUser context, CodeSyntaxConfig codeSyntax, IMediatorHandler mediator)
    {
        _context = context; _codeSyntax = codeSyntax;
        _mediator = mediator;
    }
    [HttpGet("create-product-topic/{topic}")]
    public async Task<IActionResult> CreateProductTopic(string topic, CancellationToken cancellationToken)
    {
        var ev = new AddProductTopicEvent()
        {
            Id = Guid.NewGuid(),
            Topic = topic
        };
        ev.AggregateId = _context.UserId;
        ev.Tenant = _context.Tenant;
        ev.Data = _context.Data;
        ev.Data_Zone = _context.Data_Zone;

        _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
        return Ok();
    }
    [HttpGet("publish-product-topic-item")]
    public async Task<IActionResult> PublishProductTopicItem(CancellationToken cancellationToken)
    {
        var ev = new PublishProductTopicItemEvent()
        {
            Id = Guid.NewGuid()
        };
        ev.AggregateId = _context.UserId;
        ev.Tenant = _context.Tenant;
        ev.Data = _context.Data;
        ev.Data_Zone = _context.Data_Zone;

        _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
        return Ok();
    }
    [HttpPost("add-product-cross")]
    public async Task<IActionResult> AddProductCross([FromBody] AddProductCrossRequest request, CancellationToken cancellationToken)
    {
        var productId = Guid.NewGuid();
        var productCode = request.Code;
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            productCode = await _mediator.Send(new GetCodeQuery(_codeSyntax.PROD, 1));
        }
        else
        {
            var useCodeCommand = new UseCodeCommand(_codeSyntax.PROD, productCode);
            _ = _mediator.SendCommand(useCodeCommand);
        }

        var ev = new AddProductCrossEvent()
        {
            Id = productId,
            Code = request.Code,
            ProductType = "MH",
            Condition = request.Condition,
            UnitType = request.UnitType,
            UnitCode = request.UnitCode,
            Name = request.Name,
            SourceLink = request.SourceLink,
            SourceCode = request.SourceCode,
            ShortDescription = request.ShortDescription,
            FullDescription = request.FullDescription,
            LimitedToStores = request.LimitedToStores,
            Channel = request.Channel,
            Channel_Category = request.Channel_Category,
            Origin = request.Origin,
            Brand = request.Brand,
            Manufacturer = request.Manufacturer,
            ManufacturerNumber = request.ManufacturerNumber,
            Image = request.Image,
            Images = request.Images,
            Gtin = request.Gtin,
            ProductCost = request.ProductCost,
            CurrencyCost = request.CurrencyCost,
            Price = request.Price,
            Currency = request.Currency,
            IsTaxExempt = request.IsTaxExempt,
            Tax = request.Tax,
            OrderMinimumQuantity = request.OrderMinimumQuantity,
            OrderMaximumQuantity = request.OrderMaximumQuantity,
            IsShipEnabled = request.IsShipEnabled,
            IsFreeShipping = request.IsFreeShipping,
            ProductTag = request.ProductTag
        };
        _ = _mediator.PublishEvent(ev, PublishStrategy.ParallelNoWait, cancellationToken);
        return Ok();
    }

}
