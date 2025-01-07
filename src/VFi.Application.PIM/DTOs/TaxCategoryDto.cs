

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class TaxCategoryDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int? Type { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public string? UpdatedByName { get; set; }
    }
    public class ListTaxDto : ListBoxDto
    {
        public double? Rate { get; set; }
    }

}
