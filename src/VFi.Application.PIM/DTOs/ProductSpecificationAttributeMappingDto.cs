
namespace VFi.Application.PIM.DTOs
{
    public class ProductSpecificationAttributeMappingDto
    {
        public Guid? Id { get; set; }
        public Guid? SpecificationAttributeOptionId { get; set; }
        public Guid? SpecificationAttributeId { get; set; }
        public Guid? ProductId { get; set; }
        public string? OptionName { get; set; }
        public string? OptionCode { get; set; }
        public decimal? OptionNumberValue { get; set; }
        public string? OptionColor { get; set; }
        public string? SpecificationAttributeName { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductSpecificationAttributeMappingQueryParams
    {
        public string? SpecificationAttributeOptionId { get; set; }
        public string? ProductId { get; set; }
    }
}
