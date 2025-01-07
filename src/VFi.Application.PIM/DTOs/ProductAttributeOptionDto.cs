

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ProductAttributeOptionDto
    {
        public Guid? Id { get; set; }
        public Guid ProductAttributeId { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }
        public decimal PriceAdjustment { get; set; }
        public decimal WeightAdjustment { get; set; }
        public bool IsPreSelected { get; set; }
        public int DisplayOrder { get; set; }
        public int? ValueTypeId { get; set; }
        public Guid? LinkedProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
    }
    public class DeleteProductAttributeOptionDto
    {
        public Guid Id { get; set; }
    }
}