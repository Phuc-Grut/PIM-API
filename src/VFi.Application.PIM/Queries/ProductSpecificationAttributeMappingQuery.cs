using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductSpecificationAttributeMappingQueryAll : IQuery<IEnumerable<ProductSpecificationAttributeMappingDto>>
    {
        public ProductSpecificationAttributeMappingQueryAll()
        {
        }
    }

    public class ProductSpecificationAttributeMappingQueryById : IQuery<ProductSpecificationAttributeMappingDto>
    {
        public ProductSpecificationAttributeMappingQueryById()
        {
        }

        public ProductSpecificationAttributeMappingQueryById(Guid productSpecificationAttributeMappingId)
        {
            ProductSpecificationAttributeMappingId = productSpecificationAttributeMappingId;
        }

        public Guid ProductSpecificationAttributeMappingId { get; set; }
    }
    public class ProductSpecificationAttributeMappingQueryCheckExist : IQuery<bool>
    {

        public ProductSpecificationAttributeMappingQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductSpecificationAttributeMappingPagingQuery : ListQuery, IQuery<PagingResponse<ProductSpecificationAttributeMappingDto>>
    {
        public ProductSpecificationAttributeMappingPagingQuery( ProductSpecificationAttributeMappingQueryParams productSpecificationAttributeMappingQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productSpecificationAttributeMappingQueryParams;
        }

        public ProductSpecificationAttributeMappingPagingQuery(ProductSpecificationAttributeMappingQueryParams productSpecificationAttributeMappingQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productSpecificationAttributeMappingQueryParams;
        }

        public ProductSpecificationAttributeMappingQueryParams QueryParams { get; set; }
    }

    public class ProductSpecificationAttributeMappingQueryHandler :
                                             IQueryHandler<ProductSpecificationAttributeMappingQueryAll, IEnumerable<ProductSpecificationAttributeMappingDto>>,
                                             IQueryHandler<ProductSpecificationAttributeMappingQueryCheckExist, bool>,
                                             IQueryHandler<ProductSpecificationAttributeMappingQueryById, ProductSpecificationAttributeMappingDto>,
                                             IQueryHandler<ProductSpecificationAttributeMappingPagingQuery, PagingResponse<ProductSpecificationAttributeMappingDto>>
    {
        private readonly IProductSpecificationAttributeMappingRepository _productSpecificationAttributeMappingRepository;
        private readonly ISpecificationAttributeOptionRepository _specificationAttributeOptionRepository;
        private readonly ISpecificationAttributeRepository _specificationAttributeRepository;
        public ProductSpecificationAttributeMappingQueryHandler(IProductSpecificationAttributeMappingRepository productSpecificationAttributeMappingRespository,
            ISpecificationAttributeOptionRepository specificationAttributeOptionRepository,
            ISpecificationAttributeRepository specificationAttributeRepository
            )
        {
            _productSpecificationAttributeMappingRepository = productSpecificationAttributeMappingRespository;
            _specificationAttributeOptionRepository = specificationAttributeOptionRepository;
            _specificationAttributeRepository = specificationAttributeRepository;
        }
        public async Task<bool> Handle(ProductSpecificationAttributeMappingQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productSpecificationAttributeMappingRepository.CheckExistById(request.Id);
        }
        public async Task<ProductSpecificationAttributeMappingDto> Handle(ProductSpecificationAttributeMappingQueryById request, CancellationToken cancellationToken)
        {
            var productSpecificationAttributeMapping = await _productSpecificationAttributeMappingRepository.GetById(request.ProductSpecificationAttributeMappingId);
            var result = new ProductSpecificationAttributeMappingDto()
            {
                Id = productSpecificationAttributeMapping.Id,
                SpecificationAttributeOptionId = productSpecificationAttributeMapping.SpecificationAttributeOptionId,
                ProductId = productSpecificationAttributeMapping.ProductId,
                DisplayOrder = productSpecificationAttributeMapping.DisplayOrder
            };
            return result;
        }

        public async Task<PagingResponse<ProductSpecificationAttributeMappingDto>> Handle(ProductSpecificationAttributeMappingPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductSpecificationAttributeMappingDto>();
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            if (request.QueryParams.SpecificationAttributeOptionId != null)
            {
                filter.Add("specificationAttributeOptionId", request.QueryParams.SpecificationAttributeOptionId);
            }
            var count = await _productSpecificationAttributeMappingRepository.FilterCount(filter);
            var productSpecificationAttributeMappings = await _productSpecificationAttributeMappingRepository.Filter(filter, request.PageSize, request.PageIndex);
            var specificationAttributeOptions = await _specificationAttributeOptionRepository.GetAll();
            var specificationAttributes = await _specificationAttributeRepository.GetAll();
            var data = productSpecificationAttributeMappings.Select(productSpecificationAttributeMapping =>
            {
                var specificationAttributeOption = specificationAttributeOptions.Where(x => x.Id == productSpecificationAttributeMapping.SpecificationAttributeOptionId).FirstOrDefault();
                var specificationAttribute = specificationAttributes.Where(x => x.Id == specificationAttributeOption.SpecificationAttributeId).FirstOrDefault();
                return new ProductSpecificationAttributeMappingDto()
                {
                    Id = productSpecificationAttributeMapping.Id,
                    SpecificationAttributeId = productSpecificationAttributeMapping.SpecificationAttributeId,
                    SpecificationAttributeOptionId = productSpecificationAttributeMapping.SpecificationAttributeOptionId,
                    ProductId = productSpecificationAttributeMapping.ProductId,
                    DisplayOrder = productSpecificationAttributeMapping.DisplayOrder,
                    OptionName = specificationAttributeOption is not null ? specificationAttributeOption.Name : "",
                    OptionCode = specificationAttributeOption is not null ? specificationAttributeOption.Code : "",
                    OptionNumberValue = specificationAttributeOption is not null ? specificationAttributeOption.NumberValue : 0,
                    OptionColor = specificationAttributeOption is not null ? specificationAttributeOption.Color : "",
                    SpecificationAttributeName = specificationAttribute is not null ? specificationAttribute.Name : ""
                };
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductSpecificationAttributeMappingDto>> Handle(ProductSpecificationAttributeMappingQueryAll request, CancellationToken cancellationToken)
        {
            var productSpecificationAttributeMappings = await _productSpecificationAttributeMappingRepository.GetAll();
            var result = productSpecificationAttributeMappings.Select(productSpecificationAttributeMapping => new ProductSpecificationAttributeMappingDto()
            {
                Id = productSpecificationAttributeMapping.Id,
                SpecificationAttributeOptionId = productSpecificationAttributeMapping.SpecificationAttributeOptionId,
                ProductId = productSpecificationAttributeMapping.ProductId,
                DisplayOrder = productSpecificationAttributeMapping.DisplayOrder
            });
            return result;
        }
    }
}
