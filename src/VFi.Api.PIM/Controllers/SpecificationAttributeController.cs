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
    public class SpecificationAttributeController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<SpecificationAttributeController> _logger;
        public SpecificationAttributeController(IMediatorHandler mediator, IContextUser context, ILogger<SpecificationAttributeController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxRequest request)
        {
            var result = await _mediator.Send(new SpecificationAttributeQueryListBox(request.Keyword, request.Status));
            return Ok(result);
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _mediator.Send(new SpecificationAttributeQueryById(id));
            return Ok(rs);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new SpecificationAttributePagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddSpecificationAttributeRequest request)
        {
            var Id = Guid.NewGuid();
            var create = _context.GetUserId();
            var createdDate = DateTime.Now;
            var addOption = request.Options?.Select(x => new SpecificationAttributeOptionDto()
            {
                SpecificationAttributeId = Id,
                Name = x.Name,
                Code = x.Code,
                NumberValue = x.NumberValue,
                Color = x.Color,
                DisplayOrder = x.DisplayOrder,
                CreatedDate = createdDate,
                CreatedBy = create
            }).ToList();

            var specificationAttributeAddCommand = new SpecificationAttributeAddCommand(
              Id,
              request.Code,
              request.Name,
              request.Alias,
              request.Description,
              request.Status,
              request.DisplayOrder,
              DateTime.Now,
              _context.GetUserId(),
              _context.FullName,
              addOption
          );
            var result = await _mediator.SendCommand(specificationAttributeAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditSpecificationAttributeRequest request)
        {
            var attributeId = new Guid(request.Id);
            var obj = await _mediator.Send(new SpecificationAttributeQueryById(attributeId));
            var useBy = _context.GetUserId();
            var useDate = DateTime.Now;

            var updateOption = request.Options?.Select(x => new SpecificationAttributeOptionDto()
            {
                Id = x.Id,
                SpecificationAttributeId = attributeId,
                Name = x.Name,
                Code = x.Code,
                NumberValue = x.NumberValue,
                Color = x.Color,
                DisplayOrder = x.DisplayOrder,
                CreatedDate = obj.CreatedDate,
                UpdatedDate = useDate,
                CreatedBy = obj.CreatedBy,
                UpdatedBy = useBy
            }).ToList();

            var specificationAttributeEditCommand = new SpecificationAttributeEditCommand(
               attributeId,
               request.Code,
               request.Name,
               request.Alias,
               request.Description,
               request.Status,
               request.DisplayOrder,
               obj.CreatedDate,
               useDate,
               obj.CreatedBy,
               _context.GetUserId(),
               obj.CreatedByName,
               _context.FullName,
               updateOption
           );

            var result = await _mediator.SendCommand(specificationAttributeEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new SpecificationAttributeSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
            var result = await _mediator.SendCommand(new SpecificationAttributeDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
