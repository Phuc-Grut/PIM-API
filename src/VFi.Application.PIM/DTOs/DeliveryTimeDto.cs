

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class DeliveryTimeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
        public int? MinDays { get; set; }
        public int? MaxDays { get; set; }
        public int? Status { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
    }
}