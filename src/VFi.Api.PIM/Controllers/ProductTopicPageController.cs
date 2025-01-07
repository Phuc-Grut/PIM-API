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
using System.Text;

namespace VFi.Api.PIM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTopicPageController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductTopicPageController> _logger;
        public ProductTopicPageController(IMediatorHandler mediator, IContextUser context, ILogger<ProductTopicPageController> logger)
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
            var result = await _mediator.Send(new ProductTopicPageQueryAll());
            return Ok(result);
        }
        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxProductTopicRequest request)
        {
            var result = await _mediator.Send(new ProductTopicPageQueryListBox(request.Status, request.Keyword));
            return Ok(result);
        }
        [HttpGet("bystatus/{status}")]
        public async Task<IActionResult> GetByStatus(int status)
        {
            
            var query = new ProductTopicPageQueryAllByStatus(); query.Status = status;

            var rs = await _mediator.Send(query);

            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(rs.Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Title,
                    x.Keywords,
                    x.Slug,
                    x.Description,
                    x.Image,
                    x.Icon,
                    x.Icon2,
                    x.Tags,
                    x.Status,
                    x.DisplayOrder
                }));


            return Ok(rs);
        }
        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] FilterQuery request)
        {
            var query = new ProductTopicPagePagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductTopicPageRequest request)
        {
            var item = new ProductTopicPageAddCommand();
            item.Id = Guid.NewGuid();
            item.Code = request.Code;
            item.Slug = request.Slug;
            item.Keywords =request.Keywords;
            item.Name = request.Name; item.Title = request.Title;
            item.Description = request.Description;
            item.Image = request.Image;
            item.Icon = request.Icon;
            item.Icon2 = request.Icon2;
          
            item.Tags = request.Tags;
           
            item.Status = request.Status;
            item.DisplayOrder = request.DisplayOrder;
            item.CreatedBy = _context.GetUserId();
            item.CreatedDate = DateTime.Now;
            item.CreatedByName = _context.FullName;
            var result = await _mediator.SendCommand(item);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductTopicPageRequest request)
        {
            var item = new ProductTopicPageEditCommand();
            item.Id = new Guid(request.Id);
            item.Code = request.Code;
            item.Name = request.Name; item.Title = request.Title;
            item.Slug = request.Slug;
            item.Description = request.Description;
            item.Keywords = request.Keywords;
            item.Image = request.Image;
            item.Icon = request.Icon;
            item.Icon2 = request.Icon2;
             
            item.Tags = request.Tags;
           
            item.Status = request.Status;
            item.DisplayOrder = request.DisplayOrder;
            item.UpdatedBy = _context.GetUserId();
            item.UpdatedDate = DateTime.Now;
            item.UpdatedByName = _context.FullName;
            var result = await _mediator.SendCommand(item);

            return Ok(result);
        }

        /// <summary>
        /// Cập nhật xắp sếp
        /// </summary>
        /// <param name="request">Thông  tin</param>
        /// <returns>notification</returns>
        //[HttpPut("sort")]
        //public async Task<IActionResult> Sort([FromBody] EditProductTopicPageSortRequest request)
        //{
        //    foreach (var item in request.ListGuid)
        //    {
        //        var ProductTopic = await _mediator.Send(new ProductTopicPageQueryById(item.Id));
        //        if (ProductTopic == null)
        //        {
        //            return BadRequest(new ValidationResult("ProductTopic not exists"));
        //        }
        //    }

        //    var datas = request.ListGuid.Select(x => new ProductTopicPageSortDto
        //    {
        //        Id = x.Id,
        //        SortOrder = x.SortOrder
        //    });

        //    var data = new EditProductTopicPageSortCommand(datas);

        //    var result = await _mediator.SendCommand(data);
        //    return Ok();
        //}

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _mediator.SendCommand(new ProductTopicPageDeleteCommand(new Guid(id)));

            return Ok(result);
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _mediator.Send(new ProductTopicPageQueryById(id));


            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new
                {
                    rs.Id,
                    rs.Code,
                    rs.Title,
                    rs.Name, 
                    rs.Keywords,
                    rs.Slug,
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
            var rs = await _mediator.Send(new ProductTopicPageQueryByCode(code));
            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new
                {
                    rs.Id,
                    rs.Code,
                    rs.Slug,
                    rs.Keywords,
                    rs.Title,
                    rs.Name, 
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
            var rs = await _mediator.Send(new ProductTopicPageQueryBySlug(slug));
            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new
                {
                    rs.Id,
                    rs.Code,
                    rs.Slug,
                    rs.Keywords,
                    rs.Title,
                    rs.Name,
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
        [HttpGet("get-topic-by-pageid/{id}")]
        public async Task<IActionResult> GetTopicByPageId(Guid id)
        {
            var items = await _mediator.Send(new ProductTopicQueryByPageId(id));
            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(items.Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Slug,
                    x.Keywords,
                    x.Name,
                    x.Title,
                    x.Description,
                    x.Image,
                    x.Icon,
                    x.Icon2,
                    x.Tags,
                    x.Status,
                    x.DisplayOrder
                }));

            return Ok(items);
        }
        [HttpGet("get-topic-by-pageslug/{slug}")]
        public async Task<IActionResult> GetTopicByPageSlug(string slug)
        {
            var items = await _mediator.Send(new ProductTopicQueryByPageSlug(slug));
            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(items.Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Slug,
                    x.Keywords,
                    x.Title,
                    x.Name,
                    x.Description,
                    x.Image,
                    x.Icon,
                    x.Icon2,
                    x.Tags,
                    x.Status,
                    x.DisplayOrder
                }));

            return Ok(items);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var sortCommand = new ProductTopicPageSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

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
