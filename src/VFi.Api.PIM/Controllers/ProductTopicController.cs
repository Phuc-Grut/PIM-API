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
    public class ProductTopicController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductTopicController> _logger;
        public ProductTopicController(IMediatorHandler mediator, IContextUser context, ILogger<ProductTopicController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <returns>List ProductTopic</returns>
        [HttpGet("get-list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _mediator.Send(new ProductTopicQueryAll());
            return Ok(result);
        }
        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductTopicRequest request)
        {
            var result = await _mediator.Send(new ProductTopicQueryListBox(request.Status, request.Keyword, request.ProductTopicPageId));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingProductTopicRequest request)
        {
            var query = new ProductTopicPagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize, request.ProductTopicPageId);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductTopicRequest request)
        {
            var item = new ProductTopicAddCommand(
                Guid.NewGuid(),
                request.Code,
                request.Name,
                request.Slug,
                request.Description,
                request.Keywords,
                request.Image,
                request.Icon,
                request.Icon2,
                request.Tags,
                request.Status,
                request.DisplayOrder,
                request.Title,
                _context.GetUserId(),
                DateTime.Now,
                _context.FullName,
                request.ProductTopicPageIds,
                request.ProductTopicPageCodes
                );
            var result = await _mediator.SendCommand(item);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductTopicRequest request)
        {
            var item = new ProductTopicEditCommand(
                request.Id,
                request.Code,
                request.Name,
                request.Slug,
                request.Description,
                request.Keywords,
                request.Image,
                request.Icon,
                request.Icon2,
                request.Tags,
                request.Status,
                request.DisplayOrder,
                request.Title,
                _context.GetUserId(),
                DateTime.Now,
                _context.FullName,
                request.ProductTopicPageIds,
                request.ProductTopicPageCodes
                );
            var result = await _mediator.SendCommand(item);

            return Ok(result);
        }

        /// <summary>
        /// Cập nhật xắp sếp
        /// </summary>
        /// <param name="request">Thông  tin</param>
        /// <returns>notification</returns>
        //[HttpPut("sort")]
        //public async Task<IActionResult> Sort([FromBody] EditProductTopicSortRequest request)
        //{
        //    foreach (var item in request.ListGuid)
        //    {
        //        var ProductTopic = await _mediator.Send(new ProductTopicQueryById(item.Id));
        //        if (ProductTopic == null)
        //        {
        //            return BadRequest(new ValidationResult("ProductTopic not exists"));
        //        }
        //    }

        //    var datas = request.ListGuid.Select(x => new ProductTopicSortDto
        //    {
        //        Id = x.Id,
        //        SortOrder = x.SortOrder
        //    });

        //    var data = new EditProductTopicSortCommand(datas);

        //    var result = await _mediator.SendCommand(data);
        //    return Ok();
        //}

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductTopicDeleteCommand(new Guid(id)));

            return Ok(result);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _mediator.Send(new ProductTopicQueryById(id));


            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new
                {
                    rs.Id,
                    rs.Code,
                    rs.Name, 
                    rs.Title,
                    rs.Slug,
                    rs.Keywords,
                    rs.Description,
                    rs.Image,
                    rs.Icon,
                    rs.Icon2, 
                    rs.Tags, 
                    rs.Status,
                    rs.DisplayOrder
                });

            return Ok(rs);
        }
        [HttpGet("get-by-code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var rs = await _mediator.Send(new ProductTopicQueryByCode(code));
            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new
                {
                    rs.Id,
                    rs.Code, 
                    rs.Name,
                    rs.Title,
                    rs.Slug,
                    rs.Keywords,
                    rs.Description,
                    rs.Image,
                    rs.Icon,
                    rs.Icon2, 
                    rs.Tags, 
                    rs.Status,
                    rs.DisplayOrder
                });

            return Ok(rs);
        }
        [HttpGet("get-by-slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug)
        {
            var rs = await _mediator.Send(new ProductTopicQueryBySlug(slug));
            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new
                {
                    rs.Id,
                    rs.Code,
                    rs.Name,
                    rs.Title,
                    rs.Slug,
                    rs.Keywords,
                    rs.Description,
                    rs.Image,
                    rs.Icon,
                    rs.Icon2,
                    rs.Tags,
                    rs.Status,
                    rs.DisplayOrder
                });

            return Ok(rs);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new ProductTopicSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
