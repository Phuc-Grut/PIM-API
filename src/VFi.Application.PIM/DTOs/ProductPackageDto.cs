

namespace VFi.Application.PIM.DTOs
{
    public class ProductPackageDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }
        public string? CreatedByName { get; set; }
    }
    public class ProductPackageQueryParams
    {
        public string? ProductId { get; set; }
    }
}
