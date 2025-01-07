using Consul;
using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantAttributeCombinationController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductVariantAttributeCombinationController> _logger;
        public ProductVariantAttributeCombinationController(IMediatorHandler mediator, IContextUser context, ILogger<ProductVariantAttributeCombinationController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductVariantAttributeCombinationRequest request)
        {
            var result = await _mediator.Send(new ProductVariantAttributeCombinationQueryListBox(request.ToBaseQuery()));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductVariantAttributeCombinationRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductVariantAttributeCombinationPagingQuery query = new ProductVariantAttributeCombinationPagingQuery(request.ToBaseQuery(), pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductVariantAttributeCombinationRequest request)
        {
            var productSpecificationAttributeMappingAddCommand = new ProductVariantAttributeCombinationAddCommand(
                Guid.NewGuid(),
                request.Name,
                request.ProductId,
                request.Sku,
                request.Gtin,
                request.ManufacturerPartNumber,
                request.Price,
                request.Length,
                request.Width,
                request.Height,
                request.BasePriceAmount,
                request.BasePriceBaseAmount,
                request.AssignedMediaFileIds,
                request.IsActive,
                request.DeliveryTimeId,
                request.QuantityUnitId,
                request.AttributesXml,
                request.StockQuantity,
                request.AllowOutOfStockOrders,
                DateTime.Now,
               _context.GetUserId()
          );
            var result = await _mediator.SendCommand(productSpecificationAttributeMappingAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductVariantAttributeCombinationRequest request)
        {
            var productSpecificationAttributeMappingEditCommand = new ProductVariantAttributeCombinationEditCommand(
               Guid.NewGuid(),
                 request.Name,
                request.ProductId,
                request.Sku,
                request.Gtin,
                request.ManufacturerPartNumber,
                request.Price,
                request.Length,
                request.Width,
                request.Height,
                request.BasePriceAmount,
                request.BasePriceBaseAmount,
                request.AssignedMediaFileIds,
                request.IsActive,
                request.DeliveryTimeId,
                request.QuantityUnitId,
                request.AttributesXml,
                request.StockQuantity,
                request.AllowOutOfStockOrders,
                DateTime.Now,
               _context.GetUserId()
           );

            var result = await _mediator.SendCommand(productSpecificationAttributeMappingEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductVariantAttributeCombinationDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
