

namespace VFi.Application.PIM.DTOs
{
    public class ProductInventoryDto
    {
        public Guid Id { get; set; }
        public Guid? WarehouseId { get; set; }
        public string? WarehouseCode { get; set; }
        public string? WarehouseName { get; set; }
        public Guid? ProductId { get; set; }
        public decimal? StockQuantity { get; set; }
        public decimal? ReservedQuantity { get; set; }
        public decimal? PlannedQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public Guid? UnitId { get; set; }
        public string? UnitCode { get; set; }
        public string? UnitName { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public string? SpecificationCode1 { get; set; }
        public string? SpecificationCode10 { get; set; }
        public string? SpecificationCode2 { get; set; }
        public string? SpecificationCode3 { get; set; }
        public string? SpecificationCode4 { get; set; }
        public string? SpecificationCode5 { get; set; }
        public string? SpecificationCode6 { get; set; }
        public string? SpecificationCode7 { get; set; }
        public string? SpecificationCode8 { get; set; }
        public string? SpecificationCode9 { get; set; }
    }
    public class ProductInventoryQueryParams
    {
        public string? ProductId { get; set; }
        public string? WarehouseId { get; set; }
    }
}
