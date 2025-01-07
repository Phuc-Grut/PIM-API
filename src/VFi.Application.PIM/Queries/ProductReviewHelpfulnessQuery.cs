using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductReviewHelpfulnessQueryAll : IQuery<IEnumerable<ProductReviewHelpfulnessDto>>
    {
        public ProductReviewHelpfulnessQueryAll()
        {
        }
    }

    public class ProductReviewHelpfulnessQueryById : IQuery<ProductReviewHelpfulnessDto>
    {
        public ProductReviewHelpfulnessQueryById()
        {
        }

        public ProductReviewHelpfulnessQueryById(Guid productReviewHelpfulnessId)
        {
            ProductReviewHelpfulnessId = productReviewHelpfulnessId;
        }

        public Guid ProductReviewHelpfulnessId { get; set; }
    }
    public class ProductReviewHelpfulnessQueryCheckExist : IQuery<bool>
    {

        public ProductReviewHelpfulnessQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductReviewHelpfulnessPagingQuery : ListQuery, IQuery<PagingResponse<ProductReviewHelpfulnessDto>>
    {
        public ProductReviewHelpfulnessPagingQuery(ProductReviewHelpfulnessQueryParams productReviewHelpfulnessQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productReviewHelpfulnessQueryParams;
        }

        public ProductReviewHelpfulnessPagingQuery( ProductReviewHelpfulnessQueryParams productReviewHelpfulnessQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productReviewHelpfulnessQueryParams;
        }

        public ProductReviewHelpfulnessQueryParams QueryParams { get; set; }
    }

    public class ProductReviewHelpfulnessQueryHandler : 
                                             IQueryHandler<ProductReviewHelpfulnessQueryAll, IEnumerable<ProductReviewHelpfulnessDto>>, 
                                             IQueryHandler<ProductReviewHelpfulnessQueryCheckExist, bool>,
                                             IQueryHandler<ProductReviewHelpfulnessQueryById, ProductReviewHelpfulnessDto>, 
                                             IQueryHandler<ProductReviewHelpfulnessPagingQuery, PagingResponse<ProductReviewHelpfulnessDto>>
    {
        private readonly IProductReviewHelpfulnessRepository _productReviewHelpfulnessRepository;
        public ProductReviewHelpfulnessQueryHandler(IProductReviewHelpfulnessRepository productReviewHelpfulnessRespository)
        {
            _productReviewHelpfulnessRepository = productReviewHelpfulnessRespository;
        }
        public async Task<bool> Handle(ProductReviewHelpfulnessQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productReviewHelpfulnessRepository.CheckExistById(request.Id);
        }
        public async Task<ProductReviewHelpfulnessDto> Handle(ProductReviewHelpfulnessQueryById request, CancellationToken cancellationToken)
        {
            var productReviewHelpfulness = await _productReviewHelpfulnessRepository.GetById(request.ProductReviewHelpfulnessId);
            var result = new ProductReviewHelpfulnessDto()
            {
                Id = productReviewHelpfulness.Id,
                ProductReviewId = productReviewHelpfulness.ProductReviewId,
                WasHelpful = productReviewHelpfulness.WasHelpful
            };
            return result;
        }

        public async Task<PagingResponse<ProductReviewHelpfulnessDto>> Handle(ProductReviewHelpfulnessPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductReviewHelpfulnessDto>();
            var filter = new Dictionary<string, object>();
           
            if (!String.IsNullOrEmpty(request.QueryParams.ProductReviewId))
            {
                filter.Add("productReviewId", request.QueryParams.ProductReviewId);
            }
            var count = await _productReviewHelpfulnessRepository.FilterCount( filter);
            var productReviewHelpfulnesss = await _productReviewHelpfulnessRepository.Filter( filter, request.PageSize, request.PageIndex);
            var data = productReviewHelpfulnesss.Select(productReviewHelpfulness => new ProductReviewHelpfulnessDto()
            {
                Id = productReviewHelpfulness.Id,
                ProductReviewId = productReviewHelpfulness.ProductReviewId,
                WasHelpful = productReviewHelpfulness.WasHelpful
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductReviewHelpfulnessDto>> Handle(ProductReviewHelpfulnessQueryAll request, CancellationToken cancellationToken)
        {
            var productReviewHelpfulnesss = await _productReviewHelpfulnessRepository.GetAll();
            var result = productReviewHelpfulnesss.Select(productReviewHelpfulness => new ProductReviewHelpfulnessDto()
            {
                Id= productReviewHelpfulness.Id,
                ProductReviewId=productReviewHelpfulness.ProductReviewId,
                WasHelpful=productReviewHelpfulness.WasHelpful
            });
            return result;
        }
    }
}
