using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductAttributeMappingQueryAll : IQuery<IEnumerable<ProductAttributeMappingDto>>
    {
        public ProductAttributeMappingQueryAll()
        {
        }
    }

    public class ProductAttributeMappingQueryById : IQuery<ProductAttributeMappingDto>
    {
        public ProductAttributeMappingQueryById()
        {
        }

        public ProductAttributeMappingQueryById(Guid productAttributeMappingId)
        {
            ProductAttributeMappingId = productAttributeMappingId;
        }

        public Guid ProductAttributeMappingId { get; set; }
    }
    public class ProductAttributeMappingQueryCheckExist : IQuery<bool>
    {

        public ProductAttributeMappingQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductAttributeMappingPagingQuery : ListQuery, IQuery<PagingResponse<ProductAttributeMappingDto>>
    {
        public ProductAttributeMappingPagingQuery( ProductAttributeMappingQueryParams productAttributeMappingQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productAttributeMappingQueryParams;
        }

        public ProductAttributeMappingPagingQuery(ProductAttributeMappingQueryParams productAttributeMappingQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productAttributeMappingQueryParams;
        }

        public ProductAttributeMappingQueryParams QueryParams { get; set; }
    }

    public class ProductAttributeMappingQueryHandler :
                                             IQueryHandler<ProductAttributeMappingQueryAll, IEnumerable<ProductAttributeMappingDto>>,
                                             IQueryHandler<ProductAttributeMappingQueryCheckExist, bool>,
                                             IQueryHandler<ProductAttributeMappingQueryById, ProductAttributeMappingDto>,
                                             IQueryHandler<ProductAttributeMappingPagingQuery, PagingResponse<ProductAttributeMappingDto>>
    {
        private readonly IProductProductAttributeMappingRepository _productAttributeMappingRepository;
        private readonly IProductVariantAttributeValueRepository _productVariantAttributeValueRepository;
        private readonly IProductAttributeRepository _productAttributeRepository;
 
        public ProductAttributeMappingQueryHandler(IProductProductAttributeMappingRepository productAttributeMappingRespository,
                                                   IProductVariantAttributeValueRepository productVariantAttributeValueRepository,
                                                   IProductAttributeRepository productAttributeRepository
            )
        {
            _productAttributeMappingRepository = productAttributeMappingRespository;
            _productVariantAttributeValueRepository=productVariantAttributeValueRepository;
            _productAttributeRepository= productAttributeRepository;
        }
        public async Task<bool> Handle(ProductAttributeMappingQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productAttributeMappingRepository.CheckExistById(request.Id);
        }
        public async Task<ProductAttributeMappingDto> Handle(ProductAttributeMappingQueryById request, CancellationToken cancellationToken)
        {
            var productAttributeMapping = await _productAttributeMappingRepository.GetById(request.ProductAttributeMappingId);
            var options = await _productVariantAttributeValueRepository.GetByParentId(productAttributeMapping.Id);
            var result = new ProductAttributeMappingDto()
            {
                Id = productAttributeMapping.Id,
                ProductId = productAttributeMapping.ProductId,
                ProductAttributeId = productAttributeMapping.ProductAttributeId,
                TextPrompt = productAttributeMapping.TextPrompt,
                CustomData = productAttributeMapping.CustomData,
                IsRequired = productAttributeMapping.IsRequired,
                AttributeControlTypeId = productAttributeMapping.AttributeControlTypeId,
                DisplayOrder = productAttributeMapping.DisplayOrder,
                Options = options?.Select(a => new ProductVariantAttributeValueDto()
                {
                    Id = a.Id,
                    ProductVariantAttributeId = a.ProductVariantAttributeId,
                    Name = a.Name,
                    Alias = a.Alias,
                    Image = a.Image,
                    Color = a.Color,
                    PriceAdjustment = a.PriceAdjustment,
                    WeightAdjustment = a.WeightAdjustment,
                    DisplayOrder = a.DisplayOrder,
                    Code = a.Code,
                    Value = a.Id,
                    Label = a.Name
                }).ToList()
            };
            return result;
        }

        public async Task<PagingResponse<ProductAttributeMappingDto>> Handle(ProductAttributeMappingPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductAttributeMappingDto>();
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            if (request.QueryParams.ProductAttributeId != null)
            {
                filter.Add("productAttributeId", request.QueryParams.ProductAttributeId);
            }
            var count = await _productAttributeMappingRepository.FilterCount(filter);
            var productAttributeMappings = await _productAttributeMappingRepository.Filter(filter, request.PageSize, request.PageIndex);
            var options = await _productVariantAttributeValueRepository.GetByParentId(productAttributeMappings);
            var productAttributes = await _productAttributeRepository.GetAll();
            var data = productAttributeMappings.Select(productAttributeMapping =>
            {
                return new ProductAttributeMappingDto()
                {
                    Id = productAttributeMapping.Id,
                    Name = productAttributes.Where(a => a.Id == productAttributeMapping.ProductAttributeId).FirstOrDefault()?.Name,
                    Alias = productAttributes.Where(a => a.Id == productAttributeMapping.ProductAttributeId).FirstOrDefault()?.Alias,
                    ProductId = productAttributeMapping.ProductId,
                    ProductAttributeId = productAttributeMapping.ProductAttributeId,
                    TextPrompt = productAttributeMapping.TextPrompt,
                    CustomData = productAttributeMapping.CustomData,
                    IsRequired = productAttributeMapping.IsRequired,
                    AttributeControlTypeId = productAttributeMapping.AttributeControlTypeId,
                    DisplayOrder = productAttributeMapping.DisplayOrder,
                    Options = options.Where(x=>x.ProductVariantAttributeId == productAttributeMapping.Id).Select(a => new ProductVariantAttributeValueDto()
                    {
                        Id = a.Id,
                        ProductVariantAttributeId = a.ProductVariantAttributeId,
                        Name = a.Name,
                        Alias = a.Alias,
                        Image = a.Image,
                        Color = a.Color,
                        PriceAdjustment = a.PriceAdjustment,
                        WeightAdjustment = a.WeightAdjustment,
                        DisplayOrder = a.DisplayOrder,
                        Code = a.Code,
                        Value = a.Id,
                        Label = a.Name
                    }).ToList() 
                };
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductAttributeMappingDto>> Handle(ProductAttributeMappingQueryAll request, CancellationToken cancellationToken)
        {
            var productAttributeMappings = await _productAttributeMappingRepository.GetAll();
            var result = productAttributeMappings.Select(productAttributeMapping => new ProductAttributeMappingDto()
            {
                Id = productAttributeMapping.Id,
                ProductId = productAttributeMapping.ProductId,
                ProductAttributeId = productAttributeMapping.ProductAttributeId,
                TextPrompt = productAttributeMapping.TextPrompt,
                CustomData = productAttributeMapping.CustomData,
                IsRequired = productAttributeMapping.IsRequired,
                AttributeControlTypeId = productAttributeMapping.AttributeControlTypeId,
                DisplayOrder = productAttributeMapping.DisplayOrder,
            });
            return result;
        }
    }
}
