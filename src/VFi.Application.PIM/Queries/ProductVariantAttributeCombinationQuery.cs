using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductVariantAttributeCombinationQueryAll : IQuery<IEnumerable<ProductVariantAttributeCombinationDto>>
    {
        public ProductVariantAttributeCombinationQueryAll()
        {
        }
    }
    public class ProductVariantAttributeCombinationQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductVariantAttributeCombinationQueryListBox(ProductVariantAttributeCombinationQueryParams productReviewQueryParams)
        {
            QueryParams = productReviewQueryParams;
        }
        public ProductVariantAttributeCombinationQueryParams QueryParams { get; set; }
    }
    public class ProductVariantAttributeCombinationQueryById : IQuery<ProductVariantAttributeCombinationDto>
    {
        public ProductVariantAttributeCombinationQueryById()
        {
        }

        public ProductVariantAttributeCombinationQueryById(Guid productVariantAttributeCombinationId)
        {
            ProductVariantAttributeCombinationId = productVariantAttributeCombinationId;
        }

        public Guid ProductVariantAttributeCombinationId { get; set; }
    }
    public class ProductVariantAttributeCombinationQueryCheckExist : IQuery<bool>
    {

        public ProductVariantAttributeCombinationQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductVariantAttributeCombinationPagingQuery : ListQuery, IQuery<PagingResponse<ProductVariantAttributeCombinationDto>>
    {
        public ProductVariantAttributeCombinationPagingQuery( ProductVariantAttributeCombinationQueryParams productVariantAttributeCombinationQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productVariantAttributeCombinationQueryParams;
        }

        public ProductVariantAttributeCombinationPagingQuery( ProductVariantAttributeCombinationQueryParams productVariantAttributeCombinationQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productVariantAttributeCombinationQueryParams;
        }

        public ProductVariantAttributeCombinationQueryParams QueryParams { get; set; }
    }

    public class ProductVariantAttributeCombinationQueryHandler : IQueryHandler<ProductVariantAttributeCombinationQueryListBox, IEnumerable<ListBoxDto>>,
                                             IQueryHandler<ProductVariantAttributeCombinationQueryAll, IEnumerable<ProductVariantAttributeCombinationDto>>,
                                             IQueryHandler<ProductVariantAttributeCombinationQueryCheckExist, bool>,
                                             IQueryHandler<ProductVariantAttributeCombinationQueryById, ProductVariantAttributeCombinationDto>,
                                             IQueryHandler<ProductVariantAttributeCombinationPagingQuery, PagingResponse<ProductVariantAttributeCombinationDto>>
    {
        private readonly IProductVariantAttributeCombinationRepository _productVariantAttributeCombinationRepository;
        public ProductVariantAttributeCombinationQueryHandler(IProductVariantAttributeCombinationRepository productVariantAttributeCombinationRespository)
        {
            _productVariantAttributeCombinationRepository = productVariantAttributeCombinationRespository;
        }
        public async Task<bool> Handle(ProductVariantAttributeCombinationQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productVariantAttributeCombinationRepository.CheckExistById(request.Id);
        }
        public async Task<ProductVariantAttributeCombinationDto> Handle(ProductVariantAttributeCombinationQueryById request, CancellationToken cancellationToken)
        {
            var productVariantAttributeCombination = await _productVariantAttributeCombinationRepository.GetById(request.ProductVariantAttributeCombinationId);
            var result = new ProductVariantAttributeCombinationDto()
            {
                Id = productVariantAttributeCombination.Id,
                Name = productVariantAttributeCombination.Name,
                QuantityUnitId = productVariantAttributeCombination.QuantityUnitId,
                DeliveryTimeId = productVariantAttributeCombination.DeliveryTimeId,
                AllowOutOfStockOrders = productVariantAttributeCombination.AllowOutOfStockOrders,
                AssignedMediaFileIds = productVariantAttributeCombination.AssignedMediaFileIds,
                AttributesXml = productVariantAttributeCombination.AttributesXml,
                BasePriceAmount = productVariantAttributeCombination.BasePriceAmount,
                BasePriceBaseAmount = productVariantAttributeCombination.BasePriceBaseAmount,
                CreatedBy = productVariantAttributeCombination.CreatedBy,
                CreatedDate = productVariantAttributeCombination.CreatedDate,
                Gtin = productVariantAttributeCombination.Gtin,
                Height = productVariantAttributeCombination.Height,
                Length = productVariantAttributeCombination.Length,
                ManufacturerPartNumber = productVariantAttributeCombination.ManufacturerPartNumber,
                Price = productVariantAttributeCombination.Price,
                IsActive = productVariantAttributeCombination.IsActive,
                Sku = productVariantAttributeCombination.Sku,
                StockQuantity = productVariantAttributeCombination.StockQuantity,
                UpdatedBy = productVariantAttributeCombination.UpdatedBy,
                UpdatedDate = productVariantAttributeCombination.UpdatedDate,
                Width = productVariantAttributeCombination.Width,
                ProductId = productVariantAttributeCombination.ProductId,
            };
            return result;
        }

        public async Task<PagingResponse<ProductVariantAttributeCombinationDto>> Handle(ProductVariantAttributeCombinationPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductVariantAttributeCombinationDto>();
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            if (request.QueryParams.DeliveryTimeId != null)
            {
                filter.Add("deliveryTimeId", request.QueryParams.DeliveryTimeId);
            }
            if (request.QueryParams.QuantityUnitId != null)
            {
                filter.Add("quantityUnitId", request.QueryParams.QuantityUnitId);
            }
            var count = await _productVariantAttributeCombinationRepository.FilterCount(filter);
            var productVariantAttributeCombinations = await _productVariantAttributeCombinationRepository.Filter(filter, request.PageSize, request.PageIndex);
            var data = productVariantAttributeCombinations.Select(productVariantAttributeCombination => new ProductVariantAttributeCombinationDto()
            {
                Id = productVariantAttributeCombination.Id,
                Name = productVariantAttributeCombination.Name,   
                QuantityUnitId = productVariantAttributeCombination.QuantityUnitId,
                DeliveryTimeId= productVariantAttributeCombination.DeliveryTimeId,
                AllowOutOfStockOrders= productVariantAttributeCombination.AllowOutOfStockOrders,
                AssignedMediaFileIds= productVariantAttributeCombination.AssignedMediaFileIds,
                AttributesXml=productVariantAttributeCombination.AttributesXml,
                BasePriceAmount= productVariantAttributeCombination.BasePriceAmount,
                BasePriceBaseAmount= productVariantAttributeCombination.BasePriceBaseAmount,
                CreatedBy= productVariantAttributeCombination.CreatedBy,
                CreatedDate=productVariantAttributeCombination.CreatedDate,
                Gtin=productVariantAttributeCombination.Gtin,
                Height=productVariantAttributeCombination.Height,
                Length=productVariantAttributeCombination.Length,
                ManufacturerPartNumber=productVariantAttributeCombination.ManufacturerPartNumber,
                Price=productVariantAttributeCombination.Price,
                Sku=productVariantAttributeCombination.Sku,
                StockQuantity=productVariantAttributeCombination.StockQuantity,
                IsActive = productVariantAttributeCombination.IsActive,
                UpdatedBy = productVariantAttributeCombination.UpdatedBy,
                UpdatedDate=productVariantAttributeCombination.UpdatedDate,
                Width=productVariantAttributeCombination.Width,
                ProductId = productVariantAttributeCombination.ProductId,
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductVariantAttributeCombinationDto>> Handle(ProductVariantAttributeCombinationQueryAll request, CancellationToken cancellationToken)
        {
            var productVariantAttributeCombinations = await _productVariantAttributeCombinationRepository.GetAll();
            var result = productVariantAttributeCombinations.Select(productVariantAttributeCombination => new ProductVariantAttributeCombinationDto()
            {
                Id = productVariantAttributeCombination.Id,
                Name = productVariantAttributeCombination.Name,
                QuantityUnitId = productVariantAttributeCombination.QuantityUnitId,
                DeliveryTimeId = productVariantAttributeCombination.DeliveryTimeId,
                AllowOutOfStockOrders = productVariantAttributeCombination.AllowOutOfStockOrders,
                AssignedMediaFileIds = productVariantAttributeCombination.AssignedMediaFileIds,
                AttributesXml = productVariantAttributeCombination.AttributesXml,
                BasePriceAmount = productVariantAttributeCombination.BasePriceAmount,
                BasePriceBaseAmount = productVariantAttributeCombination.BasePriceBaseAmount,
                CreatedBy = productVariantAttributeCombination.CreatedBy,
                CreatedDate = productVariantAttributeCombination.CreatedDate,
                Gtin = productVariantAttributeCombination.Gtin,
                Height = productVariantAttributeCombination.Height,
                Length = productVariantAttributeCombination.Length,
                ManufacturerPartNumber = productVariantAttributeCombination.ManufacturerPartNumber,
                Price = productVariantAttributeCombination.Price,
                Sku = productVariantAttributeCombination.Sku,
                StockQuantity = productVariantAttributeCombination.StockQuantity,
                UpdatedBy = productVariantAttributeCombination.UpdatedBy,
                UpdatedDate = productVariantAttributeCombination.UpdatedDate,
                Width = productVariantAttributeCombination.Width,
                ProductId = productVariantAttributeCombination.ProductId,
                IsActive = productVariantAttributeCombination.IsActive,
            });
            return result;
        }
        public async Task<IEnumerable<ListBoxDto>> Handle(ProductVariantAttributeCombinationQueryListBox request, CancellationToken cancellationToken)
        {
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.ProductId != null)
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            if (request.QueryParams.DeliveryTimeId != null)
            {
                filter.Add("deliveryTimeId", request.QueryParams.DeliveryTimeId);
            }
            if (request.QueryParams.QuantityUnitId != null)
            {
                filter.Add("quantityUnitId", request.QueryParams.QuantityUnitId);
            }
            var productAttributeOptions = await _productVariantAttributeCombinationRepository.GetListListBox(filter);
            var result = productAttributeOptions.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Sku
            });
            return result;
        }
    }
}
