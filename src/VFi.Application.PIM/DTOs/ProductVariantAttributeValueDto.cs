

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ProductVariantAttributeValueDto
    {
        public Guid Id { get; set; }
        public Guid ProductVariantAttributeId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Image { get; set; } = null!;
        public string? Color { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public int DisplayOrder { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }
        public Guid Value { get; set; }
        public string Label { get; set; }
    }
}