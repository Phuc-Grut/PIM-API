using Consul;
using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Domain;
using VFi.NetDevPack.Mediator;
using MassTransit.Mediator;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using PuppeteerSharp;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using static VFi.Application.PIM.Commands.ProductCommand;

namespace VFi.Api.PIM.Controllers
{

    public partial class ProductController : ControllerBase
    {
        [HttpPost("add-from-link")]
        public async Task<IActionResult> AddFromLink([FromBody] CrawlerProductRequest request)
        {
            var productId = Guid.NewGuid();
            var Code = request.Code;
            int UsedStatus = 1;
            if (request.IsAuto == 1)
            {
                Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
            }
            else
            {
                var useCodeCommand = new UseCodeCommand(request.ModuleCode, Code);
                _mediator.SendCommand(useCodeCommand);
            }
            var productAddFromLinkCommand = new ProductAddFromLinkCommand(
                productId,
                Code,
                request.Link,
                UsedStatus,
                 _context.GetUserId(),
                 _context.UserName
           );
            var result = await _mediator.SendCommand(productAddFromLinkCommand);
            if (result.IsValid == false && request.IsAuto == 1 && result.Errors[0].ToString() == "ProductCode AlreadyExists")
            {
                int loopTime = 5;
                for (int i = 0; i < loopTime; i++)
                {
                    productAddFromLinkCommand.Code = await _mediator.Send(new GetCodeQuery(request.ModuleCode, UsedStatus));
                    var res = await _mediator.SendCommand(productAddFromLinkCommand);
                    if (res.IsValid == true)
                    {
                        return await HandleResult(res, productAddFromLinkCommand.Id);
                    }
                }
            }
            return await HandleResult(result, productAddFromLinkCommand.Id);
        }
        [HttpPost("add-cross")]
        public async Task<IActionResult> AddCross([FromBody] AddProductCrossRequest request)
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

            var cmd = new ProductCrossCommand()
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
            var result = await _mediator.SendCommand(cmd);
            var output = new ValidationResult<Object>(result);
            //output.SetValue(cmd);
            return Ok(output);
        }
    }

}