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
    public class ProductTagController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductTagController> _logger;
        public ProductTagController(IMediatorHandler mediator, IContextUser context, ILogger<ProductTagController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductTagRequest request)
        {
            var result = await _mediator.Send(new ProductTagQueryListBox(request.Status, request.Type, request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new ProductTagPagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductTagRequest request)
        {
            var productTagAddCommand = new ProductTagAddCommand(
              Guid.NewGuid(),
              request.Name,
              request.Status,
              request.Type,
              _context.GetUserId(),
              DateTime.Now
          );
            var result = await _mediator.SendCommand(productTagAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductTagRequest request)
        {
            var productTagEditCommand = new ProductTagEditCommand(
               new Guid(request.Id),
               request.Name,
               request.Status,
               request.Type,
               _context.GetUserId(),
               DateTime.Now
           );

            var result = await _mediator.SendCommand(productTagEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductTagDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
