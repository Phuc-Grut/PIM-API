
namespace VFi.Application.PIM.DTOs
{
    public class ProductVariantAttributeCombinationDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public Guid ProductId { get; set; }

        public string? Sku { get; set; }

        public string? Gtin { get; set; }

        public string? ManufacturerPartNumber { get; set; }

        public decimal? Price { get; set; }

        public decimal? Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public decimal? BasePriceAmount { get; set; }

        public int? BasePriceBaseAmount { get; set; }

        public string? AssignedMediaFileIds { get; set; }

        public bool IsActive { get; set; }

        public Guid? DeliveryTimeId { get; set; }

        public Guid? QuantityUnitId { get; set; }

        public string? AttributesXml { get; set; }

        public int StockQuantity { get; set; }

        public bool AllowOutOfStockOrders { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
    }

    public class ProductVariantAttributeCombinationQueryParams
    {
        public string? QuantityUnitId { get; set; }
        public string? DeliveryTimeId { get; set; }
        public string? ProductId { get; set; }
    }
}
