using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductMediaQueryAll : IQuery<IEnumerable<ProductMediaDto>>
    {
        public ProductMediaQueryAll()
        {
        }
    }

    public class ProductMediaQueryById : IQuery<ProductMediaDto>
    {
        public ProductMediaQueryById()
        {
        }

        public ProductMediaQueryById(Guid productMediaId)
        {
            ProductMediaId = productMediaId;
        }

        public Guid ProductMediaId { get; set; }
    }
    public class ProductMediaQueryCheckExist : IQuery<bool>
    {

        public ProductMediaQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductMediaPagingQuery : ListQuery, IQuery<PagingResponse<ProductMediaDto>>
    {
        public ProductMediaPagingQuery(string? keyword, ProductMediaQueryParams productMediaQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
            QueryParams = productMediaQueryParams;
        }

        public ProductMediaPagingQuery(string? keyword, ProductMediaQueryParams productMediaQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            Keyword = keyword;
            QueryParams = productMediaQueryParams;
        }

        public string? Keyword { get; set; }
        public ProductMediaQueryParams QueryParams { get; set; }
    }

    public class ProductMediaQueryHandler :
                                             IQueryHandler<ProductMediaQueryAll, IEnumerable<ProductMediaDto>>, 
                                             IQueryHandler<ProductMediaQueryCheckExist, bool>,
                                             IQueryHandler<ProductMediaQueryById, ProductMediaDto>, 
                                             IQueryHandler<ProductMediaPagingQuery, PagingResponse<ProductMediaDto>>
    {
        private readonly IProductMediaRepository _productMediaRepository;
        public ProductMediaQueryHandler(IProductMediaRepository productMediaRespository)
        {
            _productMediaRepository = productMediaRespository;
        }
        public async Task<bool> Handle(ProductMediaQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productMediaRepository.CheckExistById(request.Id);
        }
        public async Task<ProductMediaDto> Handle(ProductMediaQueryById request, CancellationToken cancellationToken)
        {
            var productMedia = await _productMediaRepository.GetById(request.ProductMediaId);
            var result = new ProductMediaDto()
            {
               Id = productMedia.Id,
               DisplayOrder= productMedia.DisplayOrder,
               ProductId=productMedia.ProductId,
               UpdatedDate=productMedia.UpdatedDate,
               UpdatedBy=productMedia.UpdatedBy,
               CreatedBy=productMedia.CreatedBy,
               CreatedDate=productMedia.CreatedDate,
               MediaType=productMedia.MediaType,
               Name=productMedia.Name,
               Path = productMedia.Path
            };
            return result;
        }

        public async Task<PagingResponse<ProductMediaDto>> Handle(ProductMediaPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductMediaDto>();
            var filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId))
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            var count = await _productMediaRepository.FilterCount(request.Keyword, filter);
            var productMedias = await _productMediaRepository.Filter(request.Keyword, filter, request.PageSize, request.PageIndex);
            var data = productMedias.Select(productMedia => new ProductMediaDto()
            {
                Id = productMedia.Id,
                DisplayOrder = productMedia.DisplayOrder,
                ProductId = productMedia.ProductId,
                UpdatedDate = productMedia.UpdatedDate,
                UpdatedBy = productMedia.UpdatedBy,
                CreatedBy = productMedia.CreatedBy,
                CreatedDate = productMedia.CreatedDate,
                MediaType = productMedia.MediaType,
                Name = productMedia.Name,
                Path = productMedia.Path
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductMediaDto>> Handle(ProductMediaQueryAll request, CancellationToken cancellationToken)
        {
            var productMedias = await _productMediaRepository.GetAll();
            var result = productMedias.Select(productMedia => new ProductMediaDto()
            {
                Id = productMedia.Id,
                DisplayOrder = productMedia.DisplayOrder,
                ProductId = productMedia.ProductId,
                UpdatedDate = productMedia.UpdatedDate,
                UpdatedBy = productMedia.UpdatedBy,
                CreatedBy = productMedia.CreatedBy,
                CreatedDate = productMedia.CreatedDate,
                MediaType = productMedia.MediaType,
                Name = productMedia.Name,
                Path = productMedia.Path
            });
            return result;
        }
    }
}
