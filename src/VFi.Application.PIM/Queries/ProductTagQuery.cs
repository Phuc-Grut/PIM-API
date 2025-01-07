using Consul;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductTagQueryAll : IQuery<IEnumerable<ProductTagDto>>
    {
        public ProductTagQueryAll()
        {
        }
    }

    public class ProductTagQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductTagQueryListBox(int? status, int? type, string? keyword)
        {
            Keyword = keyword;
            Status = status;
            Type = type;
        }
        public int? Status { get; set; }
        public int? Type { get; set; }
        public string? Keyword { get; set; }
    }
    public class ProductTagQueryById : IQuery<ProductTagDto>
    {
        public ProductTagQueryById()
        {
        }

        public ProductTagQueryById(Guid productTagId)
        {
            ProductTagId = productTagId;
        }

        public Guid ProductTagId { get; set; }
    }
    public class ProductTagQueryCheckExist : IQuery<bool>
    {

        public ProductTagQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductTagPagingQuery : FopQuery, IQuery<PagedResult<List<ProductTagDto>>>
    {
        public ProductTagPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string? Keyword { get; set; }
    }

    public class ProductTagQueryHandler : IQueryHandler<ProductTagQueryListBox, IEnumerable<ListBoxDto>>, 
                                             IQueryHandler<ProductTagQueryAll, IEnumerable<ProductTagDto>>, 
                                             IQueryHandler<ProductTagQueryById, ProductTagDto>, 
                                             IQueryHandler<ProductTagQueryCheckExist, bool>, 
                                             IQueryHandler<ProductTagPagingQuery, PagedResult<List<ProductTagDto>>>
    {
        private readonly IProductTagRepository _productTagRepository;
        public ProductTagQueryHandler(IProductTagRepository productTagRespository)
        {
            _productTagRepository = productTagRespository;
        }
        public async Task<bool> Handle(ProductTagQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productTagRepository.CheckExistById(request.Id);
        }
        public async Task<ProductTagDto> Handle(ProductTagQueryById request, CancellationToken cancellationToken)
        {
            var productTag = await _productTagRepository.GetById(request.ProductTagId);
            var result = new ProductTagDto()
            {
                Id = productTag.Id,
                Name = productTag.Name,
                Status= productTag.Status,
                Type = productTag.Type,
                CreatedBy = productTag.CreatedBy,
                CreatedDate = productTag.CreatedDate,
                UpdatedBy = productTag.UpdatedBy,
                UpdatedDate = productTag.UpdatedDate
            };
            return result;
        }

        public async Task<PagedResult<List<ProductTagDto>>> Handle(ProductTagPagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<ProductTag>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<ProductTagDto>>();
            var (datas, count) = await _productTagRepository.Filter(request.Keyword, fopRequest);
            var data = datas.Select(productTag => new ProductTagDto()
            {
                Id = productTag.Id,
                Name = productTag.Name,
                Status = productTag.Status,
                Type = productTag.Type,
                CreatedBy = productTag.CreatedBy,
                CreatedDate = productTag.CreatedDate,
                UpdatedBy = productTag.UpdatedBy,
                UpdatedDate = productTag.UpdatedDate
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ProductTagDto>> Handle(ProductTagQueryAll request, CancellationToken cancellationToken)
        {
            var productTags = await _productTagRepository.GetAll();
            var result = productTags.Select(productTag => new ProductTagDto()
            {
                Id = productTag.Id,
                Name = productTag.Name,
                Status = productTag.Status,
                Type = productTag.Type,
                CreatedBy = productTag.CreatedBy,
                CreatedDate = productTag.CreatedDate,
                UpdatedBy = productTag.UpdatedBy,
                UpdatedDate = productTag.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductTagQueryListBox request, CancellationToken cancellationToken)
        {

            var productTags = await _productTagRepository.GetListListBox(request.Status, request.Type, request.Keyword);
            var result = productTags.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
