using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductSpecificationCodeQueryAll : IQuery<IEnumerable<ProductSpecificationCodeDto>>
    {
        public ProductSpecificationCodeQueryAll()
        {
        }
    }

    public class ProductSpecificationCodeQueryById : IQuery<ProductSpecificationCodeDto>
    {
        public ProductSpecificationCodeQueryById()
        {
        }

        public ProductSpecificationCodeQueryById(Guid productSpecificationCodeId)
        {
            ProductSpecificationCodeId = productSpecificationCodeId;
        }

        public Guid ProductSpecificationCodeId { get; set; }
    }
    public class ProductSpecificationCodeQueryCheckExist : IQuery<bool>
    {

        public ProductSpecificationCodeQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductSpecificationCodePagingQuery : ListQuery, IQuery<PagingResponse<ProductSpecificationCodeDto>>
    {
        public ProductSpecificationCodePagingQuery(ProductSpecificationCodeQueryParams productSpecificationCodeQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productSpecificationCodeQueryParams;
        }

        public ProductSpecificationCodePagingQuery(ProductSpecificationCodeQueryParams productSpecificationCodeQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productSpecificationCodeQueryParams;
        }

        public ProductSpecificationCodeQueryParams QueryParams { get; set; }
    }
    public class ProductSpecificationCodePagingQueryTo : ListQuery, IQuery<PagingResponse<ProductSpecificationCodeDto>>
    {
        public ProductSpecificationCodePagingQueryTo(ProductSpecificationCodeQueryParams productSpecificationCodeQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productSpecificationCodeQueryParams;
        }

        public ProductSpecificationCodePagingQueryTo(ProductSpecificationCodeQueryParams productSpecificationCodeQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productSpecificationCodeQueryParams;
        }

        public ProductSpecificationCodeQueryParams QueryParams { get; set; }
    }

    public class ProductSpecificationCodeQueryHandler :
                                             IQueryHandler<ProductSpecificationCodeQueryAll, IEnumerable<ProductSpecificationCodeDto>>,
                                             IQueryHandler<ProductSpecificationCodeQueryCheckExist, bool>,
                                             IQueryHandler<ProductSpecificationCodeQueryById, ProductSpecificationCodeDto>,
                                             IQueryHandler<ProductSpecificationCodePagingQuery, PagingResponse<ProductSpecificationCodeDto>>,
                                             IQueryHandler<ProductSpecificationCodePagingQueryTo, PagingResponse<ProductSpecificationCodeDto>>
    {
        private readonly IProductSpecificationCodeRepository _productSpecificationCodeRepository;
        public ProductSpecificationCodeQueryHandler(IProductSpecificationCodeRepository productSpecificationCodeRepository)
        {
            _productSpecificationCodeRepository = productSpecificationCodeRepository;
        }
        public async Task<bool> Handle(ProductSpecificationCodeQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productSpecificationCodeRepository.CheckExistById(request.Id);
        }
        public async Task<ProductSpecificationCodeDto> Handle(ProductSpecificationCodeQueryById request, CancellationToken cancellationToken)
        {
            var productSpecificationCode = await _productSpecificationCodeRepository.GetById(request.ProductSpecificationCodeId);
            var result = new ProductSpecificationCodeDto()
            {
                Id = productSpecificationCode.Id,
                Name = productSpecificationCode.Name,
                DuplicateAllowed = productSpecificationCode.DuplicateAllowed,
                DataTypes = productSpecificationCode.DataTypes,
                Status = productSpecificationCode.Status,
                ProductId = productSpecificationCode.ProductId,
                DisplayOrder = productSpecificationCode.DisplayOrder
            };
            return result;
        }

        public async Task<PagingResponse<ProductSpecificationCodeDto>> Handle(ProductSpecificationCodePagingQueryTo request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductSpecificationCodeDto>();
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            var count = await _productSpecificationCodeRepository.FilterCount(filter);
            var productSpecificationCodeRepository = await _productSpecificationCodeRepository.Filter(filter, request.PageSize, request.PageIndex);
            var specificationAttributeOptions = await _productSpecificationCodeRepository.GetAll();
            //var specificationAttributes = await _productSpecificationCodeRepository.GetAll();
            var data = productSpecificationCodeRepository.Select(productSpecificationCodeRepository =>
            {
                var specificationAttributeOption = specificationAttributeOptions.Where(x => x.Id == productSpecificationCodeRepository.ProductId).FirstOrDefault();
                // var specificationAttribute = specificationAttributes.Where(x => x.Id == specificationAttributeOption.SpecificationAttributeId).FirstOrDefault();
                return new ProductSpecificationCodeDto()
                {
                    Id = productSpecificationCodeRepository.Id,
                    ProductId = productSpecificationCodeRepository.ProductId,
                    DisplayOrder = productSpecificationCodeRepository.DisplayOrder,
                    Name = "specificationCode" + productSpecificationCodeRepository.DisplayOrder.ToString(),
                    Label = productSpecificationCodeRepository is not null ? productSpecificationCodeRepository.Name : "",
                    DuplicateAllowed = productSpecificationCodeRepository is not null ? productSpecificationCodeRepository.DuplicateAllowed : false,
                    Status = productSpecificationCodeRepository is not null ? productSpecificationCodeRepository.Status : 0,
                    DataTypes = productSpecificationCodeRepository is not null ? productSpecificationCodeRepository.DataTypes : 0,
                };
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }
        public async Task<PagingResponse<ProductSpecificationCodeDto>> Handle(ProductSpecificationCodePagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductSpecificationCodeDto>();
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            var count = await _productSpecificationCodeRepository.FilterCount(filter);
            var productSpecificationCodeRepository = await _productSpecificationCodeRepository.Filter(filter, request.PageSize, request.PageIndex);
            var specificationAttributeOptions = await _productSpecificationCodeRepository.GetAll();
            //var specificationAttributes = await _productSpecificationCodeRepository.GetAll();
            var data = productSpecificationCodeRepository.Select(productSpecificationCodeRepository =>
            {
                var specificationAttributeOption = specificationAttributeOptions.Where(x => x.Id == productSpecificationCodeRepository.ProductId).FirstOrDefault();
                // var specificationAttribute = specificationAttributes.Where(x => x.Id == specificationAttributeOption.SpecificationAttributeId).FirstOrDefault();
                return new ProductSpecificationCodeDto()
                {
                    Id = productSpecificationCodeRepository.Id,
                    ProductId = productSpecificationCodeRepository.ProductId,
                    DisplayOrder = productSpecificationCodeRepository.DisplayOrder,
                    Name = productSpecificationCodeRepository.Name,
                    DuplicateAllowed = productSpecificationCodeRepository is not null ? productSpecificationCodeRepository.DuplicateAllowed : false,
                    Status = productSpecificationCodeRepository is not null ? productSpecificationCodeRepository.Status : 0,
                    DataTypes = productSpecificationCodeRepository is not null ? productSpecificationCodeRepository.DataTypes : 0,
                };
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductSpecificationCodeDto>> Handle(ProductSpecificationCodeQueryAll request, CancellationToken cancellationToken)
        {
            var productSpecificationCode = await _productSpecificationCodeRepository.GetAll();
            var result = productSpecificationCode.Select(productSpecificationCode => new ProductSpecificationCodeDto()
            {
                Id = productSpecificationCode.Id,
                Name = productSpecificationCode.Name,
                DuplicateAllowed = productSpecificationCode.DuplicateAllowed,
                Status = productSpecificationCode.Status,
                DataTypes = productSpecificationCode.DataTypes,
                ProductId = productSpecificationCode.ProductId,
                DisplayOrder = productSpecificationCode.DisplayOrder
            });
            return result;
        }
    }
}
