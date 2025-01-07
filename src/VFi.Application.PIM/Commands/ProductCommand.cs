using Consul;
using VFi.Application.PIM.Commands;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class ProductCommand : Command
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? ProductType { get; set; }
        public bool? ForBuy { get; set; }
        public bool? ForSale { get; set; }
        public bool? ForProduction { get; set; }
        public int? Condition { get; set; }
        public Guid? UnitId { get; set; }
        public string? UnitType { get; set; }
        public string? UnitCode { get; set; }
        public string Name { get; set; } = null!;
        public string? SourceLink { get; set; }
        public string SourceCode { get; set; }
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
        public string? CreatedByName { get; set; }
        public string? ProductTag { get; set; }
        public Guid? OriginId { get; set; }
        public Guid? BrandId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public Guid? ProductTypeId { get; set; }
        public string? SpecificationAttributeOptions { get; set; }
        public string? ProductAttributes { get; set; }
        public string? RelatedProducts { get; set; }
        public List<ProductServiceAddDto>? ListProductServiceAdd { get; set; }
        public List<TierPriceDto>? ListTierPrice { get; set; }
        public List<ProductServiceAddDto>? ProductServiceAdd { get; set; }
        public List<ProductInventoryDto>? ListInventory { get; set; }
        public List<ProductPackageDto>? ListPackage { get; set; }
        public List<ProductSpecificationCodeDto>? ListProductSpecificationCode { get; set; }
        public List<ProductSpecificationAttributeMappingDto>? ListProductSpecificationAttributeMapping { get; set; }
        public Guid? CategoryRootId { get; set; }
        public string? CategoryRoot { get; set; }
        public bool? LotSerial { get; set; }
        public bool? IsVariant { get; set; }
        public string? AttributesJson { get; set; }
        public int? VariantCount { get; set; }
        public Guid? ParentId { get; set; }
    }
    public class AddProductMediaVariantDto
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string? MediaType { get; set; }
        public int? DisplayOrder { get; set; }
    }
    public class ProductInventoryVariantDto
    {
        public Guid? Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public Guid? UnitId { get; set; }
        public Guid? ProductId { get; set; }
        public int? StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
        public int? PlannedQuantity { get; set; }
        public string? SpecificationCode1 { set; get; }
        public string? SpecificationCode2 { set; get; }
        public string? SpecificationCode3 { set; get; }
        public string? SpecificationCode4 { set; get; }
        public string? SpecificationCode5 { set; get; }
        public string? SpecificationCode6 { set; get; }
        public string? SpecificationCode7 { set; get; }
        public string? SpecificationCode8 { set; get; }
        public string? SpecificationCode9 { set; get; }
        public string? SpecificationCode10 { set; get; }
    }
    public class ProductPackageVariantDto
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
    }
    public class ProductVariantRequest: Command
    {   
        public Guid? Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string? AttributesJson { get; set; }
        public string? Sku { get; set; }
        public string? ManufacturerNumber { get; set; }
        public string? Gtin { get; set; }
        public decimal? Price { get; set; }
        public string? Currency { get; set; }
        public Guid? DeliveryTimeId { get; set; }
        public Guid? UnitId { get; set; }
        public string? UnitType { get; set; }
        public string? UnitCode { get; set; }
        public int? ManageInventoryMethodId { get; set; }
        public bool? MultiPacking { get; set; }
        public int? Packages { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public Guid? ActionBy { get; set; }
        public string? ActionByName { get; set; }
        public Guid ParentId { get; set; }
        public List<AddProductMediaVariantDto>? ListMedia { get; set; }
        public List<ProductInventoryVariantDto>? ListInventory { get; set; }
        public List<ProductPackageVariantDto>? ListPackage { get; set; }
        public List<ProductSpecificationCodeDto>? ListProductSpecificationCode { get; set; }
    }
    public class ProductVariantAddCommand : ProductVariantRequest
    {
        public ProductVariantAddCommand(
            Guid? Id,
            string? Code,
            string Name,
            int Status,
            string? AttributesJson,
            string? Sku,
            string? ManufacturerNumber,
            string? Gtin,
            decimal? Price,
            string? Currency,
            Guid? DeliveryTimeId,
            Guid? UnitId,
            string? UnitType,
            string? UnitCode,
            int? ManageInventoryMethodId,
            bool? MultiPacking,
            int? Packages,
            decimal? Weight,
            decimal? Length,
            decimal? Width,
            decimal? Height,
            Guid? ActionBy,
            string? ActionByName,
            Guid ParentId,
            List<AddProductMediaVariantDto>? ListMedia,
            List<ProductInventoryVariantDto>? ListInventory,
            List<ProductPackageVariantDto>? ListPackage,
            List<ProductSpecificationCodeDto>? ListProductSpecificationCode
          )
        {
            this.Id =Id;
            this.Code=Code;
            this.Name=Name;
            this.Status=Status;
            this.AttributesJson=AttributesJson;
            this.Sku=Sku;
            this.ManufacturerNumber=ManufacturerNumber;
            this.Gtin=Gtin;
            this.Price=Price;
            this.Currency=Currency;
            this.DeliveryTimeId=DeliveryTimeId;
            this.UnitId=UnitId;
            this.UnitType=UnitType;
            this.UnitCode=UnitCode;
            this.ManageInventoryMethodId=ManageInventoryMethodId;
            this.MultiPacking=MultiPacking;
            this.Packages=Packages;
            this.Weight=Weight;
            this.Length=Length;
            this.Width=Width;
            this.Height=Height;
            this.ActionBy=ActionBy;
            this.ActionByName=ActionByName;
            this.ParentId=ParentId;
            this.ListMedia=ListMedia;
            this.ListInventory=ListInventory;
            this.ListPackage =ListPackage;
            this.ListProductSpecificationCode = ListProductSpecificationCode;
        }
        public bool IsValid(IProductRepository _context)
        {
            ValidationResult = new ProductVariantAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductVariantCreateAllCommand : ProductVariantRequest
    {
        public ProductVariantCreateAllCommand(
            Guid Id
          )
        {
            this.Id = Id;
        }
    }

    public class ProductAddCommand : ProductCommand
    {
        public string? CreatedByName { get; set; }
        public ProductAddCommand(
            Guid Id,
            string? Code,
            string? ProductType,
            bool? ForBuy,
            bool? ForSale,
            bool? ForProduction,
            int? Condition,
            Guid? UnitId,
            string? UnitType,
            string? UnitCode,
            string Name,
            string? SourceLink,
            string? ShortDescription,
            string? FullDescription,
            string? LimitedToStores,
            string? IdGroupCategories,
            List<ProductCategoriesDto>? Categories,
            string? IdCategories,
            string? Origin,
            string? Brand,
            string? Manufacturer,
            string? ManufacturerNumber,
            string? Image,
            string? Gtin,
            decimal? ProductCost,
            string? CurrencyCost,
            decimal? Price,
            bool? HasTierPrices,
            string? Currency,
            bool? IsTaxExempt,
            Guid? TaxCategoryId,
            bool? IsEsd,
            int? OrderMinimumQuantity,
            int? OrderMaximumQuantity,
            int? QuantityStep,
            int? ManageInventoryMethodId,
            bool? UseMultipleWarehouses,
            Guid? WarehouseId,
            string? Sku,
            int? StockQuantity,
            int? ReservedQuantity,
            int? PlannedQuantity,
            int? Packages,
            decimal? Weight,
            decimal? Length,
            decimal? Width,
            decimal? Height,
            Guid? DeliveryTimeId,
            bool? IsShipEnabled,
            bool? IsFreeShipping,
            decimal? AdditionalShippingCharge,
            bool? CanReturn,
            string? CustomsTariffNumber,
            bool? Deleted,
            int? Status,
            Guid? CreatedBy,
            DateTime? CreatedDate,
            string? ProductTag,
            Guid? OriginId,
            Guid? BrandId,
            Guid? ManufacturerId,
            Guid? ProductTypeId,
            Guid? CategoryRootId,
            string? CategoryRoot,
            string? createdName
          )
        {
            this.Id = Id;
            this.Code = Code;
            this.ProductType = ProductType;
            this.ForBuy = ForBuy;
            this.ForSale = ForSale;
            this.SourceLink = SourceLink;
            this.ForProduction = ForProduction;
            this.Condition = Condition;
            this.UnitId = UnitId;
            this.UnitType = UnitType;
            this.UnitCode = UnitCode;
            this.Name = Name;
            this.ShortDescription = ShortDescription;
            this.FullDescription = FullDescription;
            this.LimitedToStores = LimitedToStores;
            this.IdGroupCategories = IdGroupCategories;
            this.Categories = Categories;
            this.IdCategories = IdCategories;
            this.Origin = Origin;
            this.Brand = Brand;
            this.Manufacturer = Manufacturer;
            this.ManufacturerNumber = ManufacturerNumber;
            this.Image = Image;
            this.Gtin = Gtin;
            this.ProductCost = ProductCost;
            this.CurrencyCost = CurrencyCost;
            this.Price = Price;
            this.HasTierPrices = HasTierPrices;
            this.Currency = Currency;
            this.IsTaxExempt = IsTaxExempt;
            this.TaxCategoryId = TaxCategoryId;
            this.IsEsd = IsEsd;
            this.OrderMinimumQuantity = OrderMinimumQuantity;
            this.OrderMaximumQuantity = OrderMaximumQuantity;
            this.QuantityStep = QuantityStep;
            this.ManageInventoryMethodId = ManageInventoryMethodId;
            this.UseMultipleWarehouses = UseMultipleWarehouses;
            this.WarehouseId = WarehouseId;
            this.Sku = Sku;
            this.StockQuantity = StockQuantity;
            this.ReservedQuantity = ReservedQuantity;
            this.PlannedQuantity = PlannedQuantity;
            this.Packages = Packages;
            this.Weight = Weight;
            this.Length = Length;
            this.Width = Width;
            this.Height = Height;
            this.DeliveryTimeId = DeliveryTimeId;
            this.IsShipEnabled = IsShipEnabled;
            this.IsFreeShipping = IsFreeShipping;
            this.AdditionalShippingCharge = AdditionalShippingCharge;
            this.CanReturn = CanReturn;
            this.CustomsTariffNumber = CustomsTariffNumber;
            this.Deleted = Deleted;
            this.Status = Status;
            this.CreatedBy = CreatedBy;
            this.CreatedDate = CreatedDate;
            this.ProductTag = ProductTag;
            this.OriginId = OriginId;
            this.BrandId = BrandId;
            this.ManufacturerId = ManufacturerId;
            this.ProductTypeId = ProductTypeId;
            this.CategoryRootId = CategoryRootId;
            this.CategoryRoot = CategoryRoot;
            CreatedByName = createdName;
        }
        public bool IsValid(IProductRepository _context)
        {
            ValidationResult = new ProductAddCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductAddCompactCommand : ProductCommand
    {
        public ProductAddCompactCommand(
            Guid Id,
            string? Code,
            string Name,
            Guid? ProductTypeId,
            string? ProductType,
            Guid? CategoryRootId,
            string? CategoryRoot,
            bool? ForBuy,
            bool? ForSale,
            bool? ForProduction,
            int? Condition,
            Guid? UnitId,
            string? UnitType,
            string? UnitCode,
            Guid? OriginId,
            string? Origin,
            string? Image,
            int? Status,
            decimal? Price,
            string? Currency,
            decimal? ProductCost,
            string? CurrencyCost
          )
        {
            this.Id = Id;
            this.Code = Code;
            this.Name = Name;
            this.ProductTypeId = ProductTypeId;
            this.ProductType = ProductType;
            this.CategoryRootId = CategoryRootId;
            this.CategoryRoot = CategoryRoot;
            this.ForBuy = ForBuy;
            this.ForSale = ForSale;
            this.ForProduction = ForProduction;
            this.Condition = Condition;
            this.UnitId = UnitId;
            this.UnitType = UnitType;
            this.UnitCode = UnitCode;
            this.OriginId = OriginId;
            this.Origin = Origin;
            this.Image = Image;
            this.Status = Status;
            this.Price = Price;
            this.Currency = Currency;
            this.ProductCost = ProductCost;
            this.CurrencyCost = CurrencyCost;
        }
        public bool IsValid(IProductRepository _context)
        {
            ValidationResult = new ProductAddCompactCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ProductEditCommand : ProductCommand
    {
        public string? UpdatedByName { get; set; }
        public ProductEditCommand(
            Guid Id,
            string? Code,
            string? ProductType,
            bool? ForBuy,
            bool? ForSale,
            bool? ForProduction,
            int? Condition,
            Guid? UnitId,
            string? UnitType,
            string? UnitCode,
            string Name,
            string? SourceLink,
            string? ShortDescription,
            string? FullDescription,
            string? LimitedToStores,
            string? IdGroupCategories,
            List<ProductCategoriesDto>? Categories,
            string? IdCategories,
            string? Origin,
            string? Brand,
            string? Manufacturer,
            string? ManufacturerNumber,
            string? Image,
            string? Gtin,
            decimal? ProductCost,
            string? CurrencyCost,
            decimal? Price,
            bool? HasTierPrices,
            string? Currency,
            bool? IsTaxExempt,
            Guid? TaxCategoryId,
            bool? IsEsd,
            int? OrderMinimumQuantity,
            int? OrderMaximumQuantity,
            int? QuantityStep,
            int? ManageInventoryMethodId,
            bool? UseMultipleWarehouses,
            Guid? WarehouseId,
            string? Sku,
            int? StockQuantity,
            int? ReservedQuantity,
            int? PlannedQuantity,
            bool? MultiPacking,
            int? Packages,
            decimal? Weight,
            decimal? Length,
            decimal? Width,
            decimal? Height,
            Guid? DeliveryTimeId,
            bool? IsShipEnabled,
            bool? IsFreeShipping,
            decimal? AdditionalShippingCharge,
            bool? CanReturn,
            string? CustomsTariffNumber,
            bool? Deleted,
            int? Status,
            Guid? CreatedBy,
            Guid? UpdatedBy,
            DateTime? CreatedDate,
            DateTime? UpdatedDate,
            string? ProductTag,
            Guid? OriginId,
            Guid? BrandId,
            Guid? ManufacturerId,
            Guid? ProductTypeId,
             Guid? CategoryRootId,
            string? CategoryRoot,
            string? SpecificationAttributeOptions,
            string? ProductAttributes,
            string? RelatedProducts,
            string? updatedName,
            List<ProductInventoryDto>? listInventory,
            List<ProductPackageDto>? listPackage,
            List<ProductSpecificationCodeDto>? listProductSpecificationCode,
            List<ProductSpecificationAttributeMappingDto>? listProductSpecificationAttributeMapping,
            List<TierPriceDto>? listTierPrice,
            List<ProductServiceAddDto>? listProductServiceAdd
        )
        {
            this.Id = Id;
            this.Code = Code;
            this.ProductType = ProductType;
            this.ForBuy = ForBuy;
            this.ForSale = ForSale;
            this.ForProduction = ForProduction;
            this.Condition = Condition;
            this.UnitId = UnitId;
            this.UnitType = UnitType;
            this.UnitCode = UnitCode;
            this.Name = Name;
            this.SourceLink = SourceLink;
            this.SourceCode = SourceCode;
            this.ShortDescription = ShortDescription;
            this.FullDescription = FullDescription;
            this.LimitedToStores = LimitedToStores;
            this.IdGroupCategories = IdGroupCategories;
            this.Categories = Categories;
            this.IdCategories = IdCategories;
            this.Origin = Origin;
            this.Brand = Brand;
            this.Manufacturer = Manufacturer;
            this.ManufacturerNumber = ManufacturerNumber;
            this.Image = Image;
            this.Gtin = Gtin;
            this.ProductCost = ProductCost;
            this.CurrencyCost = CurrencyCost;
            this.Price = Price;
            this.HasTierPrices = HasTierPrices;
            this.Currency = Currency;
            this.IsTaxExempt = IsTaxExempt;
            this.TaxCategoryId = TaxCategoryId;
            this.IsEsd = IsEsd;
            this.OrderMinimumQuantity = OrderMinimumQuantity;
            this.OrderMaximumQuantity = OrderMaximumQuantity;
            this.QuantityStep = QuantityStep;
            this.ManageInventoryMethodId = ManageInventoryMethodId;
            this.UseMultipleWarehouses = UseMultipleWarehouses;
            this.WarehouseId = WarehouseId;
            this.Sku = Sku;
            this.StockQuantity = StockQuantity;
            this.ReservedQuantity = ReservedQuantity;
            this.PlannedQuantity = PlannedQuantity;
            this.MultiPacking = MultiPacking;
            this.Packages = Packages;
            this.Weight = Weight;
            this.Length = Length;
            this.Width = Width;
            this.Height = Height;
            this.DeliveryTimeId = DeliveryTimeId;
            this.IsShipEnabled = IsShipEnabled;
            this.IsFreeShipping = IsFreeShipping;
            this.AdditionalShippingCharge = AdditionalShippingCharge;
            this.CanReturn = CanReturn;
            this.CustomsTariffNumber = CustomsTariffNumber;
            this.Deleted = Deleted;
            this.Status = Status;
            this.CreatedBy = CreatedBy;
            this.UpdatedBy = UpdatedBy;
            this.CreatedDate = CreatedDate;
            this.UpdatedDate = UpdatedDate;
            this.ProductTag = ProductTag;
            this.OriginId = OriginId;
            this.BrandId = BrandId;
            this.ManufacturerId = ManufacturerId;
            this.ProductTypeId = ProductTypeId;
            this.CategoryRootId = CategoryRootId;
            this.CategoryRoot = CategoryRoot;
            this.SpecificationAttributeOptions = SpecificationAttributeOptions;
            this.ProductAttributes = ProductAttributes;
            this.RelatedProducts = RelatedProducts;
            UpdatedByName = updatedName;
            ListInventory = listInventory;
            ListPackage = listPackage;
            ListProductSpecificationCode = listProductSpecificationCode;
            ListProductSpecificationAttributeMapping = listProductSpecificationAttributeMapping;
            ListTierPrice = listTierPrice;
            ListProductServiceAdd = listProductServiceAdd;
        }
        public bool IsValid(IProductRepository _context)
        {
            ValidationResult = new ProductEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    public class ProductDuplicateCommand : ProductCommand
    {
        public ProductDuplicateCommand(
            Guid id,
            string? code,
            string name,
            int status,
            Guid? createdBy,
            string? createdByName
        )
        {
            Id = id;
            Code = code;
            Name = name;
            Status = status;
            CreatedBy = createdBy;
            CreatedByName = createdByName;

        }
        public bool IsValid(IProductRepository _context)

        {
            ValidationResult = new ProductDuplicateCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }
    
    public class ProductDeleteCommand : ProductCommand
    {
        public ProductDeleteCommand(Guid id)
        {
            Id = id;
        }
        public bool IsValid(IProductRepository _context)
        {
            ValidationResult = new ProductDeleteCommandValidation(_context).Validate(this);
            return ValidationResult.IsValid;
        }
    }



    public class ProductSizeEditCommand : ProductCommand
    {
        public ProductSizeEditCommand(
            Guid Id,
            decimal? Weight,
            decimal? Length,
            decimal? Width,
            decimal? Height
        )
        {
            this.Id = Id;
            this.Weight = Weight;
            this.Length = Length;
            this.Width = Width;
            this.Height = Height;
        }
        public bool IsValid(IProductRepository _context)
        {
            ValidationResult = new ProductSizeEditCommandValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        } 

    }

    public class ProductCrossCommand : Command
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string ProductType { get;  set; }

        /// <summary>
        /// New = 0, Refurbished = 10, Used = 20, Damaged = 30
        /// </summary>
        public int Condition { get; set; }
        public string? UnitType { get; set; }
        public string? UnitCode { get; set; }
        public string Name { get; set; } = null!;
        public string SourceLink { get; set; }
        public string SourceCode { get; set; }
        public string? ShortDescription { get; set; }
        public string? FullDescription { get; set; }
        public string? LimitedToStores { get;  set; }


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

        public bool IsValid(IProductRepository _context)
        {
            ValidationResult = new ProductCrossValidation(_context, Id).Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
