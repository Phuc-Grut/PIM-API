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
    public class ProductTopicQueryController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductTopicQueryController> _logger;

        public ProductTopicQueryController(IMediatorHandler mediator, IContextUser context, ILogger<ProductTopicQueryController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new ProductTopicQuery_PagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductTopicQueryRequest request)
        {
            var result = await _mediator.Send(new ProductTopicQuery_QueryListBox(request.Status, request.Keyword, request.ProductTopicId));
            return Ok(result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _mediator.Send(new ProductTopicQuery_QueryById(id));

            return Ok(rs);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductTopicQueryRequest request)
        {
            var data = new AddProductTopicQueryCommand(
              Guid.NewGuid(),
              request.ProductTopicId,
              request.Name,
              request.Title,
              request.Description,
              request.SourceCode,
              request.SourcePath,
              request.Keyword,
              request.Category,
              request.Seller,
              request.BrandId,
              request.Status,
              request.DisplayOrder,
              request.Condition,
              request.ProductType,
              request.PageQuery,
              request.SortQuery,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
          );
            var result = await _mediator.SendCommand(data);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductTopicQueryRequest request)
        {
            var productTypeEditCommand = new EditProductTopicQueryCommand(
              request.Id,
              request.ProductTopicId,
              request.Name,
              request.Title,
              request.Description,
              request.SourceCode,
              request.SourcePath,
              request.Keyword,
              request.Category,
              request.Seller,
              request.BrandId,
              request.Status,
              request.DisplayOrder,
              request.Condition,
              request.ProductType,
              request.PageQuery,
              request.SortQuery,
              _context.GetUserId(),
              DateTime.Now,
              _context.UserName
           );

            var result = await _mediator.SendCommand(productTypeEditCommand);

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new DeleteProductTopicQueryCommand(new Guid(id)));

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new ProductTopicQuerySortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(sortCommand);
                return Ok(result);
            }
            else
            {
                return BadRequest("Please input list sort");
            }
        }
    }
}
