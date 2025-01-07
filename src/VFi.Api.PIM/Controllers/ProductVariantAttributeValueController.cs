using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantAttributeValueController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductVariantAttributeValueController> _logger;
        public ProductVariantAttributeValueController(IMediatorHandler mediator, IContextUser context, ILogger<ProductVariantAttributeValueController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductVariantAttributeValueRequest request)
        {
            var result = await _mediator.Send(new ProductVariantAttributeValueQueryListBox(request.ProductVariantAttributeId, request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductVariantAttributeValueRequest request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            ProductVariantAttributeValuePagingQuery query = new ProductVariantAttributeValuePagingQuery(request.Keyword, request.ProductVariantAttributeId, pageSize, pageIndex);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductVariantAttributeValueRequest request)
        {
            var productVariantAttributeValueAddCommand = new ProductVariantAttributeValueAddCommand(
              Guid.NewGuid(),
              request.ProductVariantAttributeId,
              request.Code,
              request.Name,
              request.Alias,
              request.Image,
              request.Color,
              request.PriceAdjustment,
              request.WeightAdjustment,
              request.DisplayOrder,
              _context.GetUserId(),
              DateTime.Now
          );
            var result = await _mediator.SendCommand(productVariantAttributeValueAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductVariantAttributeValueRequest request)
        {
            var productVariantAttributeValueEditCommand = new ProductVariantAttributeValueEditCommand(
               new Guid(request.Id),
               request.ProductVariantAttributeId,
               request.Code,
              request.Name,
              request.Alias,
              request.Image,
              request.Color,
              request.PriceAdjustment,
              request.WeightAdjustment,
              request.DisplayOrder,
               _context.GetUserId(),
               DateTime.Now
           );

            var result = await _mediator.SendCommand(productVariantAttributeValueEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductVariantAttributeValueDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
