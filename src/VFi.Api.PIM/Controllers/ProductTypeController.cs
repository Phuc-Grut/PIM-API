using VFi.Api.PIM.ViewModels;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Application.PIM.Queries;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController: ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductTypeController> _logger;
        public ProductTypeController(IMediatorHandler mediator, IContextUser context, ILogger<ProductTypeController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductTypeRequest request)
        {
            var result = await _mediator.Send(new ProductTypeQueryListBox(request.Status, request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new ProductTypePagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductTypeRequest request)
        {
            var productTypeAddCommand = new ProductTypeAddCommand(
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
            var result = await _mediator.SendCommand(productTypeAddCommand);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductTypeRequest request)
        {
            var productTypeEditCommand = new ProductTypeEditCommand(
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

            var result = await _mediator.SendCommand(productTypeEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var productTypeSortCommand = new ProductTypeSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(productTypeSortCommand);
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
            var result = await _mediator.SendCommand(new ProductTypeDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
