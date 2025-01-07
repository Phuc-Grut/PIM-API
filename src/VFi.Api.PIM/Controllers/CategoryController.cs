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
    public partial class CategoryController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(IMediatorHandler mediator, IContextUser context, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-info")]
        public async Task<IActionResult> GetInfo([Required] string group, string category)
        {
            try
            {
                if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(group))
                {
                    var result = await _mediator.Send(new CategoryQueryBreadcrumb(group, category));
                    var rs = await _mediator.Send(new CategoryQueryByCode(group, category));
                    return Ok(new { Breadcrumb = result, Item = new { rs.Id, rs.Code, rs.Name, rs.Description, rs.ParentCategoryId, rs.FullName, rs.ParentIds, rs.JsonData, rs.Keywords, rs.Status } });
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception)
            {

                return Ok();
            }

        }

        [HttpGet("get-breadcrumb")]
        public async Task<IActionResult> GetBreadCrumb([Required] string group, string category)
        {

            try
            {
                if (!string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(group))
                {
                    var result = await _mediator.Send(new CategoryQueryBreadcrumb(group, category));
                    return Ok(result);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception)
            {

                return Ok();
            }

        }

        [HttpGet("get-by-level")]
        public async Task<IActionResult> GetByLevel([Required] string group, String? parent, int? level, int? levelCount)
        {

            var result = await _mediator.Send(new CategoryQueryByLevel(1, group, parent, level, levelCount));
            return Ok(result.Select(x => new {
                x.Id,
                x.Code,
                x.Name,
                x.Description,
                x.Image,
                x.ParentCategoryId,
                x.Url,
                x.GroupCategoryId,
                x.ParentIds,
                x.Level,
                x.DisplayOrder,
                x.Status,
                Icon = x.JsonObject?.FirstOrDefault(x => x.Value<string>("name") == "ICON")?.Value<string?>("value")
            }));

        }
        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxCategoryRequest request)
        {
            var result = await _mediator.Send(new CategoryQueryListBox(request.ToBaseQuery(), request.Keyword));
            return Ok(result);
        }
        [HttpGet("get-cbx")]
        public async Task<IActionResult> GetCbx([FromQuery] ListBoxCategoryRequest request)
        {
            var result = await _mediator.Send(new CategoryQueryCombobox(request.ToBaseQuery(), request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> Paging([FromQuery] PagingCategoryRequest request)
        {
            var query = new CategoryPagingQuery(request.Keyword, request.Filter, request.Order, request.PageNumber, request.PageSize, request.Status, request.GroupCategoryId, request.ParentCategoryId);

            var result = await _mediator.Send(query);
            return Ok(result);

        }


        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddCategoryRequest request)
        {
            var categoryAddCommand = new CategoryAddCommand(
              Guid.NewGuid(),
              request.Code,
              request.Name, request.FullName,
              request.Description, request.Image, request.Web,
              !String.IsNullOrEmpty(request.ParentCategoryId) ? new Guid(request.ParentCategoryId) : null,
              !String.IsNullOrEmpty(request.GroupCategoryId) ? new Guid(request.GroupCategoryId) : null,
              request.Status,
              request.DisplayOrder,
              request.Keywords,
              request.JsonData,
              _context.GetUserId(),
              DateTime.Now,
              _context.FullName

          );
            categoryAddCommand.Image = request.Image;
            categoryAddCommand.Web = request.Web;
            categoryAddCommand.Url = request.Url;
            var result = await _mediator.SendCommand(categoryAddCommand);
            return Ok(result);
        }
        /// <summary>
        /// Lấy thông tin 
        /// </summary>
        /// <param name="id">Thông tin</param>
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rs = await _mediator.Send(new CategoryQueryById(id));


            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new { rs.Id, rs.Code, rs.Name, rs.FullName, rs.Description, rs.Image, rs.Web, rs.ParentCategoryId, rs.ParentIds, rs.JsonData, rs.Keywords, rs.Status });

            return Ok(rs);
        }
        [HttpGet("get-by-code/{groupcode}/{code}")]
        public async Task<IActionResult> GetByCode(string groupcode, string code)
        {
            var rs = await _mediator.Send(new CategoryQueryByCode(groupcode, code));
            Request.Headers.TryGetValue("publising", out var publising);
            if (publising.Any() && publising.First().Equals("1"))
                return Ok(new { rs.Id, rs.Code, rs.Name, rs.Description, rs.ParentCategoryId, rs.FullName, rs.ParentIds, rs.JsonData, rs.Keywords, rs.Status });

            return Ok(rs);
        }
        /// <summary>
        /// Lấy thông tin breadcum
        /// </summary>
        [HttpGet("getAllParent/{id}")]
        public async Task<IActionResult> GetAllParent(Guid id)
        {
            var rs = await _mediator.Send(new CategoryQueryAllParent(id));
            return Ok(rs);
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditCategoryRequest request)
        {
            var categoryEditCommand = new CategoryEditCommand(
               new Guid(request.Id),
               request.Code,
               request.Name, request.FullName,
               request.Description, request.Image, request.Web,
               !String.IsNullOrEmpty(request.ParentCategoryId) ? new Guid(request.ParentCategoryId) : null,
               !String.IsNullOrEmpty(request.GroupCategoryId) ? new Guid(request.GroupCategoryId) : null,
               request.Status,
               request.DisplayOrder,
                request.Keywords,
              request.JsonData,
               _context.GetUserId(),
               DateTime.Now,
               _context.FullName
           );
            categoryEditCommand.Image = request.Image;
            categoryEditCommand.Web = request.Web;
            categoryEditCommand.Url = request.Url;
            var result = await _mediator.SendCommand(categoryEditCommand);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var categorySortCommand = new CategorySortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(categorySortCommand);
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
            var result = await _mediator.SendCommand(new CategoryDeleteCommand(new Guid(id)));

            return Ok(result);
        }
    }
}
