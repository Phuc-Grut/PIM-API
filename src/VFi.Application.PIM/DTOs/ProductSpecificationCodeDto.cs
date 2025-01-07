
namespace VFi.Application.PIM.DTOs
{
    public class ProductSpecificationCodeDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
        public bool DuplicateAllowed { get; set; }
        public int Status { get; set; }
        public int DataTypes { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class ProductSpecificationCodeQueryParams
    {
        public string? ProductId { get; set; }
    }
}
