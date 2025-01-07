

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class UnitDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string Description { get; set; }
        public string DisplayLocale { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; }
        public Guid? GroupUnitId { get; set; }
        public string? GroupUnitName { get; set; }
        public string? GroupUnitCode { get; set; }
        public double? Rate { get; set; }
        public int Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
    }
    public class ListBoxUnitDto
    {
        public Guid Value { get; set; }
        public string Label { get; set; }
        public string Key { get; set; }
        public Guid? GroupUnitId { get; set; }
    }
}
