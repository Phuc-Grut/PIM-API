

namespace VFi.Application.PIM.DTOs
{
    public class ProductAttributeMappingDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public Guid ProductAttributeId { get; set; }
        public Guid ProductId { get; set; }
        public string? TextPrompt { get; set; }
        public string? CustomData { get; set; }
        public bool IsRequired { get; set; }
        public int AttributeControlTypeId { get; set; }
        public int DisplayOrder { get; set; }
        public List<ProductVariantAttributeValueDto>? Options { get; set; }
    }

    public class ProductAttributeMappingQueryParams
    {
        public string? ProductAttributeId { get; set; }
        public string? ProductId { get; set; }
    }
}
