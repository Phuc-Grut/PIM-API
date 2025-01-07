using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class RelatedProductQueryAll : IQuery<IEnumerable<RelatedProductDto>>
    {
        public RelatedProductQueryAll()
        {
        }
    }

    public class RelatedProductQueryById : IQuery<RelatedProductDto>
    {
        public RelatedProductQueryById()
        {
        }

        public RelatedProductQueryById(Guid tierPriceId)
        {
            RelatedProductId = tierPriceId;
        }

        public Guid RelatedProductId { get; set; }
    }
    public class RelatedProductQueryCheckExist : IQuery<bool>
    {

        public RelatedProductQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class RelatedProductPagingQuery : ListQuery, IQuery<PagingResponse<RelatedProductDto>>
    {
        public RelatedProductPagingQuery(RelatedProductQueryParams tierPriceQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = tierPriceQueryParams;
        }

        public RelatedProductPagingQuery(RelatedProductQueryParams tierPriceQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = tierPriceQueryParams;
        }

        public RelatedProductQueryParams QueryParams { get; set; }
    }

    public class RelatedProductQueryHandler : 
                                             IQueryHandler<RelatedProductQueryAll, IEnumerable<RelatedProductDto>>, 
                                             IQueryHandler<RelatedProductQueryCheckExist, bool>,
                                             IQueryHandler<RelatedProductQueryById, RelatedProductDto>, 
                                             IQueryHandler<RelatedProductPagingQuery, PagingResponse<RelatedProductDto>>
    {
        private readonly IRelatedProductRepository _tierPriceRepository;
        private readonly IProductRepository _productRepository;
        public RelatedProductQueryHandler(IRelatedProductRepository tierPriceRespository, IProductRepository productRepository)
        {
            _tierPriceRepository = tierPriceRespository;
            _productRepository = productRepository;
        }
        public async Task<bool> Handle(RelatedProductQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _tierPriceRepository.CheckExistById(request.Id);
        }
        public async Task<RelatedProductDto> Handle(RelatedProductQueryById request, CancellationToken cancellationToken)
        {
            var tierPrice = await _tierPriceRepository.GetById(request.RelatedProductId);
            var result = new RelatedProductDto()
            {
                Id = tierPrice.Id,
                ProductId1 = tierPrice.ProductId1,
                ProductId2 = tierPrice.ProductId2,
                DisplayOrder = tierPrice.DisplayOrder
            };
            return result;
        }

        public async Task<PagingResponse<RelatedProductDto>> Handle(RelatedProductPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<RelatedProductDto>();
            var filter = new Dictionary<string, object>();
          
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId1))
            {
                filter.Add("productId1", request.QueryParams.ProductId1);
            } 
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId2))
            {
                filter.Add("productId2", request.QueryParams.ProductId2);
            }
            var count = await _tierPriceRepository.FilterCount(filter);
            var relatedProducts = await _tierPriceRepository.Filter(filter, request.PageSize, request.PageIndex);
            var listProduct = await _productRepository.Filter(relatedProducts);
            var data = listProduct.Select(product => {
                var relatedProduct = relatedProducts.Where(x => x.ProductId2 == product.Id).FirstOrDefault();
                return new RelatedProductDto()
                {
                    Id = relatedProduct.Id,
                    Name = product.Name,
                    Code = product.Code,
                    Image = product.Image,
                    ProductId1 = relatedProduct.ProductId1,
                    ProductId2 = relatedProduct.ProductId2,
                    DisplayOrder = relatedProduct.DisplayOrder
                };
            } 
           ).OrderBy(x => x.DisplayOrder);
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<RelatedProductDto>> Handle(RelatedProductQueryAll request, CancellationToken cancellationToken)
        {
            var relatedProduct = await _tierPriceRepository.GetAll();
            var result = relatedProduct.Select(tierPrice => new RelatedProductDto()
            {
                Id = tierPrice.Id,
                ProductId1 = tierPrice.ProductId1,
                ProductId2 = tierPrice.ProductId2,
                DisplayOrder = tierPrice.DisplayOrder
            });
            return result;
        }

     
    }
}
