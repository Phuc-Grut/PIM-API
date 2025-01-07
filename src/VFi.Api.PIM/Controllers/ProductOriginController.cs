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
    public class ProductOriginController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductOriginController> _logger;
        public ProductOriginController(IMediatorHandler mediator, IContextUser context, ILogger<ProductOriginController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductOriginRequest request)
        {
            var result = await _mediator.Send(new ProductOriginQueryListBox(request.Status, request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new ProductOriginPagingQuery(request.Keyword ?? "", request.Filter ?? "", request?.Order, request.PageNumber, request.PageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductOriginRequest request)
        {
            var productOriginAddCommand = new ProductOriginAddCommand(
              Guid.NewGuid(),
              request.Code,
              request.Name,
              request.Description,
              request.Status,
              request.DisplayOrder,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(productOriginAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductOriginRequest request)
        {
            var productOriginEditCommand = new ProductOriginEditCommand(
               new Guid(request.Id),
               request.Code,
               request.Name,
               request.Description,
               request.Status,
               request.DisplayOrder,
               _context.GetUserId(),
               DateTime.Now,
              _context.UserName

           );

            var result = await _mediator.SendCommand(productOriginEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var productOriginSortCommand = new ProductOriginSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(productOriginSortCommand);
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
            var result = await _mediator.SendCommand(new ProductOriginDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
