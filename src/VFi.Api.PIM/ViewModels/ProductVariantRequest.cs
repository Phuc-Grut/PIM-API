using VFi.Application.PIM.Commands;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{   
    public class ProductVariantRequest
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
        public Guid ParentId { get; set; }
        public List<AddProductMediaVariantDto>? ListMedia { get; set; }
        public List<ProductInventoryVariantDto>? ListInventory { get; set; }
        public List<ProductPackageVariantDto>? ListPackage { get; set; }
        public List<ProductSpecificationCodeDto>? ListProductSpecificationCode { get; set; }
    }
    public class AddProductVariantRequest : ProductVariantRequest
    {
    }
    public class EditProductVariantRequest : ProductVariantRequest
    {
        public string Id { get; set; }
    }
}
