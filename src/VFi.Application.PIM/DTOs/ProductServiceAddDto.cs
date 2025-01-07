
namespace VFi.Application.PIM.DTOs
{
    public class ProductServiceAddDto
    {
        public Guid? Id { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ServiceAddId { get; set; }
        public int? PayRequired { get; set; }
        public decimal? Price { get; set; }
        public decimal? MaxPrice { get; set; }
        public int CalculationMethod { get; set; }
        public string? PriceSyntax { get; set; }
        public decimal? MinPrice { get; set; }
        public string? Currency { get; set; }
        public int? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public string? CreatedByName { get; set; }
        public string? ServiceAddName { get; set; }

    }
    public class ProductServiceAddQueryParams
    {
        public string? ProductId { get; set; }
        public string? ServiceAddId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
