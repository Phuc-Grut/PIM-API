

namespace VFi.Application.PIM.DTOs
{
    public class ProductMediaDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string Path { get; set; } = null!;
        public string MediaType { get; set; } = null!;
        public int? DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }

    public class ProductMediaQueryParams
    {
        public string? ProductId { get; set; }
    }
}
