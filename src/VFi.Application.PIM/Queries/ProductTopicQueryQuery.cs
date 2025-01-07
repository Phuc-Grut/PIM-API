using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Queries
{
    public class ProductTopicQuery_QueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductTopicQuery_QueryListBox(int? status, string? keyword, Guid? productTopicId)
        {
            Status = status;
            Keyword = keyword;
            ProductTopicId = productTopicId;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
        public Guid? ProductTopicId { get; set; }
    }

    public class ProductTopicQuery_QueryById : IQuery<ProductTopicQueryDto>
    {
        public ProductTopicQuery_QueryById()
        {
        }

        public ProductTopicQuery_QueryById(Guid itemId)
        {
            Id = itemId;
        }

        public Guid Id { get; set; }
    }

    public class ProductTopicQuery_PagingQuery : FopQuery, IQuery<PagedResult<List<ProductTopicQueryDto>>>
    {
        public ProductTopicQuery_PagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string? Keyword { get; set; }
    }

    public class ProductTopicQuery_QueryHandler : IQueryHandler<ProductTopicQuery_QueryListBox, IEnumerable<ListBoxDto>>,
                                                  IQueryHandler<ProductTopicQuery_QueryById, ProductTopicQueryDto>,
                                                  IQueryHandler<ProductTopicQuery_PagingQuery, PagedResult<List<ProductTopicQueryDto>>>
    {
        private readonly IProductTopicQueryRepository _repository;

        public ProductTopicQuery_QueryHandler(IProductTopicQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductTopicQueryDto> Handle(ProductTopicQuery_QueryById request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetById(request.Id);
            var result = new ProductTopicQueryDto
            {
                Id = item.Id,
                ProductTopicId = item.ProductTopicId,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
                SourceCode = item.SourceCode,
                SourcePath = item.SourcePath,
                Keyword = item.Keyword,
                Category = item.Category,
                Seller = item.Seller,
                BrandId = item.BrandId,
                Status = item.Status,
                DisplayOrder = item.DisplayOrder,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                UpdatedByName = item.UpdatedByName,
                Condition = item.Condition,
                ProductType = item.ProductType,
                PageQuery = item.PageQuery,
                SortQuery = item.SortQuery,
            };

            return result;
        }

        public async Task<PagedResult<List<ProductTopicQueryDto>>> Handle(ProductTopicQuery_PagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<ProductTopicQuery>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<ProductTopicQueryDto>>();
            var (datas, count) = await _repository.Filter(request.Keyword, fopRequest);
            var data = datas.Select(item => new ProductTopicQueryDto()
            {
                Id = item.Id,
                ProductTopicId = item.ProductTopicId,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
                SourceCode = item.SourceCode,
                SourcePath = item.SourcePath,
                Keyword = item.Keyword,
                Category = item.Category,
                Seller = item.Seller,
                BrandId = item.BrandId,
                Status = item.Status,
                DisplayOrder = item.DisplayOrder,
                Condition = item.Condition,
                ProductType = item.ProductType,
                PageQuery = item.PageQuery,
                SortQuery = item.SortQuery,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductTopicQuery_QueryListBox request, CancellationToken cancellationToken)
        {
            var items = await _repository.Filter(request.Status, request.Keyword, request.ProductTopicId);
            var result = items.Select(x => new ListBoxDto()
            {
                Key = "",
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
