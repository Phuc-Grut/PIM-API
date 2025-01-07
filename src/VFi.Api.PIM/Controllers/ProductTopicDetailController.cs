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
    public class ProductTopicDetailController : ControllerBase
    {
        private readonly IContextUser _context;
        private readonly IMediatorHandler _mediator;
        private readonly ILogger<ProductTopicDetailController> _logger;
        public ProductTopicDetailController(IMediatorHandler mediator, IContextUser context, ILogger<ProductTopicDetailController> logger)
        {
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }

        [HttpGet("get-listbox")]
        public async Task<IActionResult> Get([FromQuery] ListBoxRequest request)
        {
            var result = await _mediator.Send(new ProductTopicDetailQueryListBox(request.Keyword));
            return Ok(result);
        }

        [HttpGet("paging-by-page")]
        public async Task<IActionResult> PagingByPage([FromQuery] PagingRequestByPage request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1; 
            var query = new ProductTopicDetailPagingByPageQuery(request.Keyword,request.Page, pageSize, pageIndex);
            query.Channel= request.Channel;query.Status= request.Status;
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("paging-by-topic")]
        public async Task<IActionResult> PagingByTopic([FromQuery] PagingRequestByTopic request)
        {
            var pageSize = request.Top;
            var pageIndex = (request.Skip / (request.Top == 0 ? 10 : request.Top)) + 1;

            var query = new ProductTopicDetailPagingQuery(request.Keyword, pageSize, pageIndex);
            query.ProductTopicId = request.TopicId;
            query.Channel = request.Channel; query.Status = request.Status;
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddProductTopicDetailRequest request)
        {
            var item = new ProductTopicDetailAddCommand();
            item.Id = Guid.NewGuid();
            item.ProductTopic = request.ProductTopic;
            item.Code = request.Code;
            item.Condition = request.Condition;
            item.Unit = request.Unit;
            item.Name = request.Name;
            item.SourceLink = request.SourceLink;
            item.SourceCode = request.SourceCode;
            item.ShortDescription = request.ShortDescription;
            item.FullDescription = request.FullDescription;
            item.Origin = request.Origin;
            item.Brand = request.Brand;
            item.Manufacturer = request.Manufacturer;
            item.Image = request.Image;
            item.Images = request.Images;
            item.Price = request.Price;
            item.Currency = request.Currency;
            item.Status = request.Status;
            item.Tags = request.Tags;
            item.Exp = request.Exp;
            item.BidPrice = request.BidPrice;
            item.Tax = request.Tax;
            item.CreatedBy = _context.GetUserId();
            item.CreatedDate = DateTime.Now;
            item.CreatedByName = _context.UserName;
         
            var result = await _mediator.SendCommand(item);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Put([FromBody] EditProductTopicDetailRequest request)
        {
            var item = new ProductTopicDetailEditCommand();
            item.Id = Guid.NewGuid();
            item.ProductTopic = request.ProductTopic;
            item.Code = request.Code;
            item.Condition = request.Condition;
            item.Unit = request.Unit;
            item.Name = request.Name;
            item.SourceLink = request.SourceLink;
            item.SourceCode = request.SourceCode;
            item.ShortDescription = request.ShortDescription;
            item.FullDescription = request.FullDescription;
            item.Origin = request.Origin;
            item.Brand = request.Brand;
            item.Manufacturer = request.Manufacturer;
            item.Image = request.Image;
            item.Images = request.Images;
            item.Price = request.Price;
            item.Currency = request.Currency;
            item.Status = request.Status;
            item.Tags = request.Tags;
            item.Exp = request.Exp;
            item.BidPrice = request.BidPrice;
            item.Tax = request.Tax;
            item.UpdatedBy = _context.GetUserId();
            item.UpdatedDate = DateTime.Now;
            item.UpdatedByName = _context.UserName;
            var result = await _mediator.SendCommand(item);

            return Ok(result);
        }

        [HttpPut("sort")]
        public async Task<IActionResult> SortList([FromBody] SortRequest request)
        {
            if (request.SortList.Count > 0)
            {
                var warehouseSortCommand = new ProductTopicDetailSortCommand(request.SortList.Select(x => new SortItemDto() { Id = x.Id, SortOrder = x.SortOrder }).ToList());

                var result = await _mediator.SendCommand(warehouseSortCommand);
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
            var result = await _mediator.SendCommand(new ProductTopicDetailDeleteCommand(new Guid(id)));

            return Ok(result);
        }


        [HttpGet("loop")]
        public async Task<IActionResult> GetProductLoop(string? page, string? topic, string? keyword, string? channel, int? top)
        {
            var pageSize = top??10;
            var query = new ProductTopicDetailLoopQuery()
            {
                Keyword = keyword,Topic = topic, Channel = channel, TopicPage = page, Top = pageSize
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
