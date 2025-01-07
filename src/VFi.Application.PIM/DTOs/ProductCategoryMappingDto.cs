

namespace VFi.Application.PIM.DTOs
{
    public class ProductCategoryMappingDto
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ProductId { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductCategoryMappingQueryParams
    {
        public string? CategoryId { get; set; }
        public string? ProductId { get; set; }
        public List<string>? ListCategory { get; set; }
    }
   
}
