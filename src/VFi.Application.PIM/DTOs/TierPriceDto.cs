

namespace VFi.Application.PIM.DTOs
{
    public class TierPriceDto
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? StoreId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int CalculationMethod { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public string? CreatedByName { get; set; }
        public string? StoreName { get; set; }
    }

    public class TierPriceQueryParams
    {
        public string? ProductId { get; set; }
        public string? StoreId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
