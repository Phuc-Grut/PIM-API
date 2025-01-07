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
    public class TaxCategoryController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<TaxCategoryController> _logger;
        public TaxCategoryController(IMediatorHandler mediator, IContextUser context, ILogger<TaxCategoryController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxTaxCategoryRequest request)
        {
            var result = await _mediator.Send(new TaxCategoryQueryListBox(request.Status, request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new TaxCategoryPagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddTaxCategoryRequest request)
        {
            var taxCategoryAddCommand = new TaxCategoryAddCommand(
              Guid.NewGuid(),
              request.Code,
              request.Name,
              request.Rate,
              request.Group,
              request.Description,
              request.Status,
              request.DisplayOrder,
              request.Type,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(taxCategoryAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditTaxCategoryRequest request)
        {
            var taxCategoryEditCommand = new TaxCategoryEditCommand(
               new Guid(request.Id),
               request.Code,
               request.Name,
               request.Rate,
               request.Group,
               request.Description,
               request.Status,
               request.DisplayOrder,
               request.Type,
               _context.GetUserId(),
               DateTime.Now,
              _context.UserName
           );

            var result = await _mediator.SendCommand(taxCategoryEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new TaxCategorySortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
        { var result = await _mediator.SendCommand(new TaxCategoryDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
