using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductCategoryMappingQueryAll : IQuery<IEnumerable<ProductCategoryMappingDto>>
    {
        public ProductCategoryMappingQueryAll()
        {
        }
    }
    public class ProductCategoryMappingQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductCategoryMappingQueryListBox(ProductCategoryMappingQueryParams productQueryParams)
        {
            QueryParams = productQueryParams;
        }
        public ProductCategoryMappingQueryParams QueryParams { get; set; }
    }
    public class ProductCategoryMappingQueryById : IQuery<ProductCategoryMappingDto>
    {
        public ProductCategoryMappingQueryById()
        {
        }

        public ProductCategoryMappingQueryById(Guid productCategoryMappingId)
        {
            ProductCategoryMappingId = productCategoryMappingId;
        }

        public Guid ProductCategoryMappingId { get; set; }
    }
    public class ProductCategoryMappingQueryCheckExist : IQuery<bool>
    {

        public ProductCategoryMappingQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductCategoryMappingPagingQuery : ListQuery, IQuery<PagingResponse<ProductCategoryMappingDto>>
    {
        public ProductCategoryMappingPagingQuery(ProductCategoryMappingQueryParams productCategoryMappingQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productCategoryMappingQueryParams;
        }

        public ProductCategoryMappingPagingQuery(string? keyword, ProductCategoryMappingQueryParams productCategoryMappingQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productCategoryMappingQueryParams;
        }

        public ProductCategoryMappingQueryParams QueryParams { get; set; }
    }

    public class ProductCategoryMappingQueryHandler :
                                             IQueryHandler<ProductCategoryMappingQueryAll, IEnumerable<ProductCategoryMappingDto>>, 
                                             IQueryHandler<ProductCategoryMappingQueryCheckExist, bool>,
                                             IQueryHandler<ProductCategoryMappingQueryById, ProductCategoryMappingDto>, 
                                             IQueryHandler<ProductCategoryMappingPagingQuery, PagingResponse<ProductCategoryMappingDto>>
    {
        private readonly IProductCategoryMappingRepository _productCategoryMappingRepository;
        private readonly IProductRepository _productRepository;
        public ProductCategoryMappingQueryHandler(IProductCategoryMappingRepository productCategoryMappingRespository, IProductRepository productRepository)
        {
            _productCategoryMappingRepository = productCategoryMappingRespository;
            _productRepository = productRepository;
        }
        public async Task<bool> Handle(ProductCategoryMappingQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productCategoryMappingRepository.CheckExistById(request.Id);
        }
        public async Task<ProductCategoryMappingDto> Handle(ProductCategoryMappingQueryById request, CancellationToken cancellationToken)
        {
            var productCategoryMapping = await _productCategoryMappingRepository.GetById(request.ProductCategoryMappingId);
            var result = new ProductCategoryMappingDto()
            {
                Id = productCategoryMapping.Id,
                CategoryId = productCategoryMapping.CategoryId,
                ProductId = productCategoryMapping.ProductId,
                DisplayOrder= productCategoryMapping.DisplayOrder
            };
            return result;
        }

        public async Task<PagingResponse<ProductCategoryMappingDto>> Handle(ProductCategoryMappingPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductCategoryMappingDto>();
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
             if (request.QueryParams.CategoryId != null)
            {
                filter.Add("categoryId", request.QueryParams.CategoryId);
            }
            if (request.QueryParams.ListCategory != null)
            {
                filter.Add("listCategory", request.QueryParams.ListCategory);
            }
            var count = await _productCategoryMappingRepository.FilterCount(filter);
            var productCategoryMappings = await _productCategoryMappingRepository.Filter(filter, request.PageSize, request.PageIndex);
            var data = productCategoryMappings.Select(productCategoryMapping => new ProductCategoryMappingDto()
            {
                Id = productCategoryMapping.Id,
                CategoryId = productCategoryMapping.CategoryId,
                ProductId = productCategoryMapping.ProductId,
                DisplayOrder = productCategoryMapping.DisplayOrder
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductCategoryMappingDto>> Handle(ProductCategoryMappingQueryAll request, CancellationToken cancellationToken)
        {
            var productCategoryMappings = await _productCategoryMappingRepository.GetAll();
            var result = productCategoryMappings.Select(productCategoryMapping => new ProductCategoryMappingDto()
            {
                Id = productCategoryMapping.Id,
                CategoryId = productCategoryMapping.CategoryId,
                ProductId = productCategoryMapping.ProductId,
                DisplayOrder = productCategoryMapping.DisplayOrder
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductCategoryMappingQueryListBox request, CancellationToken cancellationToken)
        {
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            if (request.QueryParams.CategoryId != null)
            {
                filter.Add("categoryId", request.QueryParams.CategoryId);
            }
            if (request.QueryParams.ListCategory != null)
            {
                filter.Add("listCategory", request.QueryParams.ListCategory);
            }
            var productCategoryMappings = await _productCategoryMappingRepository.GetListListBox(filter);
            var products = await _productRepository.GetAll();

            var result = products.Where(x => productCategoryMappings.Any(y => y.ProductId == x.Id)).Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
