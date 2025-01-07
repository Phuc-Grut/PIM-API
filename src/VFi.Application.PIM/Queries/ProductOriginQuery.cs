using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductOriginQueryAll : IQuery<IEnumerable<ProductOriginDto>>
    {
        public ProductOriginQueryAll()
        {
        }
    }

    public class ProductOriginQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductOriginQueryListBox(int? status, string? keyword)
        {
            Keyword = keyword;
            Status = status;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
    }
    public class ProductOriginQueryCheckExist : IQuery<bool>
    {

        public ProductOriginQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductOriginQueryById : IQuery<ProductOriginDto>
    {
        public ProductOriginQueryById()
        {
        }

        public ProductOriginQueryById(Guid productOriginId)
        {
            ProductOriginId = productOriginId;
        }

        public Guid ProductOriginId { get; set; }
    }
    public class ProductOriginPagingQuery : FopQuery, IQuery<PagedResult<List<ProductOriginDto>>>
    {
        public ProductOriginPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public string? Keyword { get; set; }
        public string? Filter { get; set; }
        public string? Order { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class ProductOriginQueryHandler : IQueryHandler<ProductOriginQueryListBox, IEnumerable<ListBoxDto>>, 
                                             IQueryHandler<ProductOriginQueryAll, IEnumerable<ProductOriginDto>>, 
                                             IQueryHandler<ProductOriginQueryCheckExist, bool>,
                                             IQueryHandler<ProductOriginQueryById, ProductOriginDto>,
                                             IQueryHandler<ProductOriginPagingQuery, PagedResult<List<ProductOriginDto>>>
    {
        private readonly IProductOriginRepository _productOriginRepository;
        public ProductOriginQueryHandler(IProductOriginRepository productOriginRespository)
        {
            _productOriginRepository = productOriginRespository;
        }
        public async Task<bool> Handle(ProductOriginQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productOriginRepository.CheckExistById(request.Id);
        }

        public async Task<ProductOriginDto> Handle(ProductOriginQueryById request, CancellationToken cancellationToken)
        {
            var productOrigin = await _productOriginRepository.GetById(request.ProductOriginId);
            var result = new ProductOriginDto()
            {
                Id = productOrigin.Id,
                Code = productOrigin.Code,
                Name = productOrigin.Name,
                Description= productOrigin.Description,
                DisplayOrder = productOrigin.DisplayOrder,
                Status = productOrigin.Status,
                CreatedBy = productOrigin.CreatedBy,
                CreatedDate = productOrigin.CreatedDate,
                CreatedByName = productOrigin.CreatedByName,
                UpdatedBy = productOrigin.UpdatedBy,
                UpdatedDate = productOrigin.UpdatedDate,
                UpdatedByName = productOrigin.UpdatedByName

            };
            return result;
        }

        public async Task<PagedResult<List<ProductOriginDto>>> Handle(ProductOriginPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<ProductOriginDto>>();
            var fopRequest = FopExpressionBuilder<ProductOrigin>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (productOrigins, count) = await _productOriginRepository.Filter(request.Keyword, fopRequest);
            var data = productOrigins.Select(productOrigin => new ProductOriginDto()
            {
                Id = productOrigin.Id,
                Code = productOrigin.Code,
                Name = productOrigin.Name,
                Description = productOrigin.Description,
                DisplayOrder = productOrigin.DisplayOrder,
                Status = productOrigin.Status,
                CreatedBy = productOrigin.CreatedBy,
                CreatedDate = productOrigin.CreatedDate,
                CreatedByName = productOrigin.CreatedByName,
                UpdatedBy = productOrigin.UpdatedBy,
                UpdatedDate = productOrigin.UpdatedDate,
                UpdatedByName = productOrigin.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ProductOriginDto>> Handle(ProductOriginQueryAll request, CancellationToken cancellationToken)
        {
            var productOrigins = await _productOriginRepository.GetAll();
            var result = productOrigins.Select(productOrigin => new ProductOriginDto()
            {
                Id = productOrigin.Id,
                Code = productOrigin.Code,
                Name = productOrigin.Name,
                Description = productOrigin.Description,
                DisplayOrder = productOrigin.DisplayOrder,
                Status = productOrigin.Status,
                CreatedBy = productOrigin.CreatedBy,
                CreatedDate = productOrigin.CreatedDate,
                UpdatedBy = productOrigin.UpdatedBy,
                UpdatedDate = productOrigin.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductOriginQueryListBox request, CancellationToken cancellationToken)
        {

            var productOrigins = await _productOriginRepository.GetListListBox(request.Status, request.Keyword);
            var result = productOrigins.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
