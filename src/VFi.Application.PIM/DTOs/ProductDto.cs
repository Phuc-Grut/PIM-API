

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    
    public class ProductPublish
    {
        public ProductPublish()
        {
        }

        //SimpleProduct = 0, GroupedProduct = 10,Reservation = 20,BundledProduct = 30,Auction = 40
        public string ProductType { get; set; }
        public Guid? Id { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// New = 0, Refurbished = 10, Used = 20, Damaged = 30
        /// </summary>
        public int Condition { get; set; }
        public string SourceLink { get; set; }
        public string SourceCode { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public bool? IsTaxExempt { get; set; }
        public int TaxRate { get; set; }
        public int? StockQuantity { get; set; }
        public bool? IsStocking { get; set; }
        public bool? IsShipEnabled { get; set; } = true;
        public bool? IsFreeShipping { get; set; }=false;
        public decimal? AdditionalShippingCharge { get; set; }
        public bool? CanReturn { get; set; }
        public string Tags { get; set; }
        public bool? LotSerial { get; set; }
        public bool? IsVariant { get; set; }
        public string? AttributesJson { get; set; }
        public int? VariantCount { get; set; }
        public Guid? ParentId { get; set; }
        public  List<ProductAttribute> Attributes { get; set; } =  new List<ProductAttribute>();


    }

    public class ProductAttribute
    {
        public ProductAttribute()
        {
        }

        public ProductAttribute(string? code, string name, string? value)
        {
            Code = code;
            Name = name;
            Value = value;
        }

        public ProductAttribute(string id, string? code, string name, string? value) : this(id, code, name)
        {
            Value = value;
        }

        public string Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? Value { get; set; }
    }
    public class ProductListView
    {
        public ProductListView()
        {
        }

        public Guid? Id { get; set; }
        public string Code { get; set; }
        /// <summary>
        /// New = 0, Refurbished = 10, Used = 20, Damaged = 30
        /// </summary>
        public int Condition { get; set; }
        public string SourceLink { get; set; }
        public string SourceCode { get; set; }
        public bool? IsStocking { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Origin { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public bool? IsTaxExempt { get; set; }
        public int TaxRate { get; set; }

    }
    public class ProductQueryParams
    {
        public string? BrandId { get; set; }
        public string? OriginId { get; set; }
        public string? ProductTypeId { get; set; }
        public string? CategoryRootId { get; set; }
        public int? Status { get; set; }
        public string? TaxCategoryId { get; set; }
        public string? UnitId { get; set; }
        public string? ListProduct { get; set; }
    }

    public class ProductDto
    {
        
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Brand { get; set; }
        public string? Categories { get; set; }
        public string IdCategories { get; set; }
        public string? Sku { get; set; }
        public string? Gtin { get; set; }
        public decimal? Price { get; set; }
        public string? Manufacturer { get; set; }
        public string? LimitedToStores { get; set; }
        public string? IdGroupCategories { get; set; }
        public string GroupCategories { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? Warehouse { get; set; }
        public string? Image { get; set; }
        public decimal? BasePriceAmount { get; set; }
        public string? BasePriceMeasureUnit { get; set; }
        public int? BasePriceBaseAmount { get; set; }
        public decimal? StockQuantity { get; set; }
        public Guid? UnitId { get; set; }
        public string? ProductType { get; set; }
        public Guid? ProductTypeId { get; set; }
        public int? Status { get; set; }
        public string? UnitType { get; set; }
        public string? UnitName { get; set; }
        public string? UnitCode { get; set; }
        public decimal? ReservedQuantity { get; set; }
        public decimal? PlannedQuantity { get; set; }
        public string? CurrencyCost { get; set; }
        public string? Currency { get; set; }
        public string? Origin { get; set; }
        public Guid? OriginId { get; set; }
        public string? OriginCode { get; set; }
        public Guid? CategoryRootId { get; set; }
        public string? CategoryRoot { get; set; }
        public bool? MultiPacking { get; set; }
        public int? Packages { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public string? AttributesJson { get; set; }
        public string? SourchCode { get; set; }
        public string? SourchLink { get; set; }
        public Guid? TaxCategoryId { get; set; }
        public bool? IsSpec { set; get; }

    }

    public class ProductFullDto : ProductDto
    {
        public bool? ForBuy { get; set; }
        public bool? ForSale { get; set; }
        public bool? ForProduction { get; set; }
        public int? Condition { get; set; }
        public string? SourceLink { get; set; }
        public string SourceCode { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? IdCategories { get; set; }
        public string? GroupCategories { get; set; }
        public string? Origin { get; set; }
        public string? ManufacturerNumber { get; set; }
        public string? Gtin { get; set; }
        public decimal? ProductCost { get; set; }
        public string? CurrencyCost { get; set; }
        public bool? HasTierPrices { get; set; }
        public string? Currency { get; set; }
        public bool? IsTaxExempt { get; set; }
        public Guid? TaxCategoryId { get; set; }
     
        public bool? IsEsd { get; set; }
        public decimal? OrderMinimumQuantity { get; set; }
        public decimal? OrderMaximumQuantity { get; set; }
 
        public decimal? QuantityStep { get; set; }
    
        public decimal? ManageInventoryMethodId { get; set; }
        public bool? UseMultipleWarehouses { get; set; }
      
        public decimal? ReservedQuantity { get; set; }
        public decimal? PlannedQuantity { get; set; }
        
        public Guid? DeliveryTimeId { get; set; }
        public bool? IsShipEnabled { get; set; }
        public bool? IsFreeShipping { get; set; }
        public decimal? AdditionalShippingCharge { get; set; }
        public bool? CanReturn { get; set; }
        public string? CustomsTariffNumber { get; set; }
        public bool? Deleted { get; set; }
        public Guid? OriginId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ManufacturerId { get; set; }   
        //List mapping
        public List<ProductMappingDto>? ListStore { get; set; }
        public List<ProductMappingDto>? ListGroupCategory { get; set; }
        public List<MappingProductCategoriesDto>? ListCategory { get; set; }
        public List<ProductInventoryDto>? ListProductInventory { get; set; }
        public List<string>? ListTag { get; set; }
        public string? Combinations { get; set; }
        public string? Tags { get; set; }
        public string? Stores { get; set; }
        public string? UnitType { get; set; }
        public string? UnitTypeGuid { get; set; }
    }
    public class ProductsInventoryDto
    {
        public Guid? ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; } = null!;
        public string? WarehouseName { get; set; }
        public Guid? WarehouseId { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal? ReservedQuantity { get; set; }
        public decimal? PlannedQuantity { get; set; }
    }
        public class ProductMappingDto
    {
        public Guid? Value { get; set; }
        public int? DisplayOrder { get; set; }
    }
    public class MappingProductCategoriesDto : ProductMappingDto
    {
        public Guid? GroupCategoryId { get; set; }
        public string? Label { get; set; }
    }
    public class ProductCategoriesDto
    {
        public Guid CategoryId { get; set; }
        public Guid? GroupCategoryId { get; set; }
    }
    public class ProductResult
    {
        public Guid? Id { get; set; }
        public bool IsValid { get; set; }
    }
    public class ProductQueryPriceDto
    {
        public Guid? ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; } = null!;
        public decimal? Price { get; set; }
    }
    public class ProductDuplicateDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
    }
    public class VariantAttribute
    {
        public VariantAttribute() { }

        public VariantAttribute(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ProductPagingDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string ProductType { get; set; }
        public Guid? ProductTypeId { get; set; }
        public Guid? UnitId { get; set; }
        public string UnitType { get; set; }
        public string UnitTypeGuid { get; set; }
        public string UnitCode { get; set; }
        public string Name { get; set; }
        public string LimitedToStores { get; set; }
        public string IdGroupCategories { get; set; }
        public string GroupCategories { get; set; }
        public string Categories { get; set; }
        public string IdCategories { get; set; }
        public string Origin { get; set; }
        public Guid? OriginId { get; set; }
        public Guid? BrandId { get; set; }
        public string Brand { get; set; }
        public Guid? ManufacturerId { get; set; }
        public string Manufacturer { get; set; }
        public Guid? CategoryRootId { get; set; }
        public string CategoryRoot { get; set; }
        public string ManufacturerNumber { get; set; }
        public string Image { get; set; }
        public string Gtin { get; set; }
        public decimal? ProductCost { get; set; }
        public string CurrencyCost { get; set; }
        public decimal? Price { get; set; }
        public bool? HasTierPrices { get; set; }
        public string Currency { get; set; }
        public bool? IsTaxExempt { get; set; }
        public Guid? TaxCategoryId { get; set; }
        public bool? IsEsd { get; set; }
        public int? OrderMinimumQuantity { get; set; }
        public int? OrderMaximumQuantity { get; set; }
        public int? QuantityStep { get; set; }
        public int? ManageInventoryMethodId { get; set; }
        public bool? UseMultipleWarehouses { get; set; }
        public Guid? WarehouseId { get; set; }
        public string Warehouse { get; set; }
        public string Sku { get; set; }
        /// <summary>
        /// Số lượng tồn kho
        /// </summary>
        public int? StockQuantity { get; set; }
        /// <summary>
        /// Số lượng đặt trước
        /// </summary>
        public int? ReservedQuantity { get; set; }
        /// <summary>
        /// Số lượng kế hoạch
        /// </summary>
        public int? PlannedQuantity { get; set; }
        /// <summary>
        /// Số kiện
        /// </summary>
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
        public string CustomsTariffNumber { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Combinations { get; set; }
        public string Tags { get; set; }
        public bool? LotSerial { get; set; }
        public bool? IsVariant { get; set; }
        public string? AttributesJson { get; set; }
        public int? VariantCount { get; set; }
        public Guid? ParentId { get; set; }
    }
}
