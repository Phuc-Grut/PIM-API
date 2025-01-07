using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductRequest
    {
        public string? Code { get; set; }
        public string? ProductType { get; set; }
        public bool? ForBuy { get; set; }
        public bool? ForSale { get; set; }
        public bool? ForProduction { get; set; }
        public int? Condition { get; set; }
        public Guid? UnitId { get; set; }
        public string? UnitCode { get; set; }
        public string? UnitType { get; set; }
        public string Name { get; set; } = null!;
        public string? SourceLink { get; set; }
        public string? SourceCode { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? LimitedToStores { get; set; }
        public string? IdGroupCategories { get; set; }
        public List<ProductCategoriesDto>? Categories { get; set; }
        public string? IdCategories { get; set; }
        public string? Origin { get; set; }
        public string? Brand { get; set; }
        public string? Manufacturer { get; set; }
        public string? ManufacturerNumber { get; set; }
        public string? Image { get; set; }
        public string? Gtin { get; set; }
        public decimal? ProductCost { get; set; }
        public string? CurrencyCost { get; set; }
        public decimal? Price { get; set; }
        public bool? HasTierPrices { get; set; }
        public string? Currency { get; set; }
        public bool? IsTaxExempt { get; set; }
        public Guid? TaxCategoryId { get; set; }
        public bool? IsEsd { get; set; }
        public int? OrderMinimumQuantity { get; set; }
        public int? OrderMaximumQuantity { get; set; }
        public int? QuantityStep { get; set; }
        public int? ManageInventoryMethodId { get; set; }
        public bool? UseMultipleWarehouses { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? Sku { get; set; }
        public int? StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
        public int? PlannedQuantity { get; set; }
        public bool? MultiPacking { get; set; }
        public int? Packages { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public Guid? DeliveryTimeId { get; set; }
        public bool? IsShipEnabled { get; set; }
        public bool? IsFreeShipping { get; set; }
        public decimal? AdditionalShippingCharge { get; set; }
        public bool? CanReturn { get; set; }
        public string? CustomsTariffNumber { get; set; }
        public bool? Deleted { get; set; }
        public int? Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? ProductTag { get; set; }
        public Guid? OriginId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public Guid? ProductTypeId { get; set; }
        public Guid? CategoryRootId { get; set; }
        public string? CategoryRoot { get; set; }
        public string? SpecificationAttributeOptions { get; set; }
        public string? ProductAttributes { get; set; }
        public string? RelatedProducts { get; set; }
        public string? ModuleCode { get; set; }
        public int? IsAuto { get; set; }
        public bool? LotSerial { get; set; }
        public bool? IsVariant { get; set; }
        public string? AttributesJson { get; set; }
        public int? VariantCount { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class EditProductRequest : AddProductRequest
    {
        public string Id { get; set; } = null!;
        public List<ProductInventoryDto>? ListInventory { get; set; }
        public List<ProductPackageDto>? ListPackage { get; set; }
        public List<ProductSpecificationCodeDto>? ListProductSpecificationCode { get; set; }
        public List<ProductSpecificationAttributeMappingDto>? ListProductSpecificationAttributeMapping { get; set; }
        public List<TierPriceDto>? ListTierPrice { get; set; }
        public List<ProductServiceAddDto>? ListProductServiceAdd { get; set; }
    }
    public class DuplicateProductRequest
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int IsAuto { get; set; }
        public string? ModuleCode { get; set; }

    }
    public class SizeProductRequest
    {
        public Guid Id { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
    }
    public class CrawlerProductRequest
    {
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public string Link { get; set; }
        public int Status { get; set; }
        public int IsAuto { get; set; }
        public string? ModuleCode { get; set; }

    }
    public class ListBoxProductRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$brandId")]
        public string? BrandId { get; set; }
        [FromQuery(Name = "$originId")]
        public string? OriginId { get; set; }
        [FromQuery(Name = "$productTypeId")]
        public string? ProductTypeId { get; set; }
        [FromQuery(Name = "$productTypeId")]
        public string? TaxCategoryId { get; set; }
        [FromQuery(Name = "$unitId")]
        public string? UnitId { get; set; }

        public ProductQueryParams ToBaseQuery() => new ProductQueryParams
        {
            BrandId = BrandId,
            OriginId = OriginId,
            ProductTypeId = ProductTypeId,
            Status = Status,
            TaxCategoryId = TaxCategoryId,
            UnitId = UnitId,
        };
    }
    public class ProductRequestQueryPaging : PagingRequest
    {
        [FromQuery(Name = "$brandId")]
        public string? BrandId { get; set; }
        [FromQuery(Name = "$originId")]
        public string? OriginId { get; set; }
        [FromQuery(Name = "$productType")]
        public string? ProductTypeId { get; set; }

        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$taxCategoryId")]
        public string? TaxCategoryId { get; set; }
        [FromQuery(Name = "$unitId")]
        public string? UnitId { get; set; }
        public ProductQueryParams ToBaseQuery() => new ProductQueryParams
        {
            BrandId = BrandId,
            OriginId = OriginId,
            ProductTypeId = ProductTypeId,
            Status = Status,
            TaxCategoryId = TaxCategoryId,
            UnitId = UnitId,
        };
    }
    public class FilterProductInventoryQuery
    {
        [FromQuery(Name = "$listProduct")]
        public string? ListProduct { get; set; }
        public ProductQueryParams ToBaseQuery() => new ProductQueryParams
        {
            ListProduct = ListProduct,
        };
    }
    public class FilterProductStockQueryById
    {
        [FromQuery(Name = "$id")]
        public Guid Id { get; set; }
    }
    public class FilterProductQueryPrice
    {
        [FromQuery(Name = "$listProduct")]
        public string? ListProduct { get; set; }
        public ProductQueryParams ToBaseQuery() => new ProductQueryParams
        {
            ListProduct = ListProduct,
        };
    }
    public class FilterProductQueryInventoryDetailBylistId
    {
        [FromQuery(Name = "$listProduct")]
        public string? ListProduct { get; set; }
        public ProductQueryParams ToBaseQuery() => new ProductQueryParams
        {
            ListProduct = ListProduct,
        };
    }
    public class AddProductCrossRequest
    {
        public string? Code { get; set; }
        public string ProductType { get;protected set; } = "MH";

      
        /// <summary>
        /// New = 0, Refurbished = 10, Used = 20, Damaged = 30
        /// </summary>
        public int Condition { get; set; } = 0;
        public string? UnitType { get; set; } = "SL";
        public string? UnitCode { get; set; } = "C01";
        public string Name { get; set; } = null!;
        public string SourceLink { get; set; }
        public string SourceCode { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? LimitedToStores { get; protected set; } = "ONLVN";

        
        public string? Channel { get; set; }
        public string? Channel_Category { get; set; } 
         
        public string? Origin { get; set; }
        public string? Brand { get; set; }
        public string? Manufacturer { get; set; }
        public string? ManufacturerNumber { get; set; }
        public string? Image { get; set; }
        public string? Images { get; set; }
        public string? Gtin { get; set; }
        public decimal? ProductCost { get; set; }
        public string? CurrencyCost { get; set; }
        public decimal? Price { get; set; } 
        public string? Currency { get; set; }
         
        public bool? IsTaxExempt { get; set; }
        public int? Tax { get; set; }

      
        public int? OrderMinimumQuantity { get; set; }
        public int? OrderMaximumQuantity { get; set; }


        public bool? IsShipEnabled { get; set; } = true;
        public bool? IsFreeShipping { get; set; } = false;

     
       
        public string? ProductTag { get; set; }

        
    }

    public class ProductQueryByListCodeRequest
    {
        public string Codes { get; set; }
    }
}
