

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class StoreDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
