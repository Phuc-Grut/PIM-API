

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ProductAttributeDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public bool AllowFiltering { get; set; }
        public int? SearchType { get; set; }
        public bool? IsOption { get; set; }
        public int DisplayOrder { get; set; }
        public string? Mapping { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public int? Status { get; set; }
        public List<ProductAttributeOptionDto>? Options { get; set; }
    }
}