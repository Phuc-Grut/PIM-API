

namespace VFi.Application.PIM.DTOs
{
    public class ProductManufacturerMappingDto
    {
        public Guid Id { get; set; }
        public Guid ManufacturerId { get; set; }
        public Guid ProductId { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class ProductManufacturerMappingQueryParams
    {
        public string? ProductId { get; set; }
        public string? ManufacturerId { get; set; }
    }
}
