using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributeController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductAttributeController> _logger;
        public ProductAttributeController(IMediatorHandler mediator, IContextUser context, ILogger<ProductAttributeController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxRequest request)
        {
            var result = await _mediator.Send(new ProductAttributeQueryListBox(request.Keyword, request.Status));
            return Ok(result);
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _mediator.Send(new ProductAttributeQueryById(id));
            return Ok(rs);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new ProductAttributePagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductAttributeRequest request)
        {
            var Id = Guid.NewGuid();
            var create = _context.GetUserId();
            var createdDate = DateTime.Now;
            var createdByName = _context.FullName;
            var addOption = request.Options?.Select(x => new ProductAttributeOptionDto()
            {
                ProductAttributeId = Id,
                Name = x.Name,
                Alias = x.Alias,
                Image = x.Image,
                Color = x.Color,
                PriceAdjustment = x.PriceAdjustment,
                WeightAdjustment = x.WeightAdjustment,
                IsPreSelected = x.IsPreSelected,
                DisplayOrder = x.DisplayOrder,
                ValueTypeId = x.ValueTypeId,
                LinkedProductId = x.LinkedProductId,
                Quantity = x.Quantity,
                CreatedDate = createdDate,
                CreatedBy = create
            }).ToList();

            var productAttributeAddCommand = new ProductAttributeAddCommand(
              Id,
              request.Code,
              request.Name,
              request.Description,
              request.Alias,
              request.AllowFiltering,
              request.SearchType,
              request.IsOption,
              request.DisplayOrder,
              request.Mapping,
              createdDate,
              create,
              createdByName,
              request.Status,
              addOption
          );
            var result = await _mediator.SendCommand(productAttributeAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductAttributeRequest request)
        {
            var attributeId = new Guid(request.Id);
            var obj = await _mediator.Send(new ProductAttributeQueryById(attributeId));
            var useBy = _context.GetUserId();
            var useDate = DateTime.Now;
            var updatedByName = _context.FullName;
            var updateOption = request.Options?.Select(x => new ProductAttributeOptionDto()
            {
                Id = x.Id,
                ProductAttributeId = attributeId,
                Name = x.Name,
                Alias = x.Alias,
                Image = x.Image,
                Color = x.Color,
                PriceAdjustment = x.PriceAdjustment,
                WeightAdjustment = x.WeightAdjustment,
                IsPreSelected = x.IsPreSelected,
                DisplayOrder = x.DisplayOrder,
                ValueTypeId = x.ValueTypeId,
                LinkedProductId = x.LinkedProductId,
                Quantity = x.Quantity,
                CreatedDate = obj.CreatedDate,
                UpdatedDate = useDate,
                CreatedBy = obj.CreatedBy,
                UpdatedBy = useBy
            }).ToList();

            var productAttributeEditCommand = new ProductAttributeEditCommand(
               attributeId,
               request.Code,
               request.Name,
               request.Description,
               request.Alias,
               request.AllowFiltering,
               request.SearchType,
               request.IsOption,
               request.DisplayOrder,
               request.Mapping,
               obj.CreatedDate,
               useDate,
               obj.CreatedBy,
               _context.GetUserId(),
               obj.CreatedByName,
               updatedByName,
               request.Status,
               updateOption
           );

            var result = await _mediator.SendCommand(productAttributeEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new ProductAttributeSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(sortCommand);
                return Ok(result);
            }
            else
            {
                return BadRequest("Please input list sort");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductAttributeDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
