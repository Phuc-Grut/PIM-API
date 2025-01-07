using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductReviewQueryAll : IQuery<IEnumerable<ProductReviewDto>>
    {
        public ProductReviewQueryAll()
        {
        }
    }

    public class ProductReviewQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductReviewQueryListBox(ProductReviewQueryParams productReviewQueryParams, string? keyword)
        {
            Keyword = keyword;
            QueryParams = productReviewQueryParams;
        }
        public ProductReviewQueryParams QueryParams { get; set; }
        public string? Keyword { get; set; }
    }
    public class ProductReviewQueryById : IQuery<ProductReviewDto>
    {
        public ProductReviewQueryById()
        {
        }

        public ProductReviewQueryById(Guid productReviewId)
        {
            ProductReviewId = productReviewId;
        }

        public Guid ProductReviewId { get; set; }
    }
    public class ProductReviewQueryCheckExist : IQuery<bool>
    {

        public ProductReviewQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductReviewPagingQuery : ListQuery, IQuery<PagingResponse<ProductReviewDto>>
    {
        public ProductReviewPagingQuery(string? keyword, ProductReviewQueryParams productReviewQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
            QueryParams = productReviewQueryParams;
        }

        public ProductReviewPagingQuery(string? keyword, ProductReviewQueryParams productReviewQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            Keyword = keyword;
            QueryParams = productReviewQueryParams;
        }

        public string? Keyword { get; set; }
        public ProductReviewQueryParams QueryParams { get; set; }
    }

    public class ProductReviewQueryHandler : IQueryHandler<ProductReviewQueryListBox, IEnumerable<ListBoxDto>>, 
                                             IQueryHandler<ProductReviewQueryAll, IEnumerable<ProductReviewDto>>, 
                                             IQueryHandler<ProductReviewQueryCheckExist, bool>,
                                             IQueryHandler<ProductReviewQueryById, ProductReviewDto>, 
                                             IQueryHandler<ProductReviewPagingQuery, PagingResponse<ProductReviewDto>>
    {
        private readonly IProductReviewRepository _productReviewRepository;
        public ProductReviewQueryHandler(IProductReviewRepository productReviewRespository)
        {
            _productReviewRepository = productReviewRespository;
        }
        public async Task<bool> Handle(ProductReviewQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productReviewRepository.CheckExistById(request.Id);
        }
        public async Task<ProductReviewDto> Handle(ProductReviewQueryById request, CancellationToken cancellationToken)
        {
            var productReview = await _productReviewRepository.GetById(request.ProductReviewId);
            var result = new ProductReviewDto()
            {
                Id = productReview.Id,
                ProductId = productReview.ProductId,
                HelpfulNoTotal = productReview.HelpfulNoTotal,
                HelpfulYesTotal = productReview.HelpfulYesTotal,
                IsVerifiedPurchase = productReview.IsVerifiedPurchase,
                Rating = productReview.Rating,
                ReviewText = productReview.ReviewText,
                Title = productReview.Title
            };
            return result;
        }

        public async Task<PagingResponse<ProductReviewDto>> Handle(ProductReviewPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductReviewDto>();
            var filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId))
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            var count = await _productReviewRepository.FilterCount(request.Keyword, filter);
            var productReviews = await _productReviewRepository.Filter(request.Keyword, filter, request.PageSize, request.PageIndex);
            var data = productReviews.Select(productReview => new ProductReviewDto()
            {
                Id = productReview.Id,
                ProductId= productReview.ProductId,
                HelpfulNoTotal= productReview.HelpfulNoTotal,
                HelpfulYesTotal= productReview.HelpfulYesTotal,
                IsVerifiedPurchase= productReview.IsVerifiedPurchase,
                Rating= productReview.Rating,
                ReviewText= productReview.ReviewText,
                Title = productReview.Title
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductReviewDto>> Handle(ProductReviewQueryAll request, CancellationToken cancellationToken)
        {
            var productReviews = await _productReviewRepository.GetAll();
            var result = productReviews.Select(productReview => new ProductReviewDto()
            {
                Id = productReview.Id,
                ProductId = productReview.ProductId,
                HelpfulNoTotal = productReview.HelpfulNoTotal,
                HelpfulYesTotal = productReview.HelpfulYesTotal,
                IsVerifiedPurchase = productReview.IsVerifiedPurchase,
                Rating = productReview.Rating,
                ReviewText = productReview.ReviewText,
                Title = productReview.Title
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductReviewQueryListBox request, CancellationToken cancellationToken)
        {
            var filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId))
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            var productReviews = await _productReviewRepository.GetListListBox(filter, request.Keyword);
            var result = productReviews.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Title
            });
            return result;
        }
    }
}
