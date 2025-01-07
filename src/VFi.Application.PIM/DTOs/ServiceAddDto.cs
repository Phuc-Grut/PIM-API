

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ServiceAddDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CalculationMethod { get; set; }
        public decimal? Price { get; set; }
        public string? PriceSyntax { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public string? Currency { get; set; }
    }
}
