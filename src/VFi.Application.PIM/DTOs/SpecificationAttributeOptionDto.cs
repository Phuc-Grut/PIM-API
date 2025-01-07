

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class SpecificationAttributeOptionDto
    {
        public Guid? Id { get; set; }
        public Guid SpecificationAttributeId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public SpecificationAttribute SpecificationAttribute { get; set; }
        public decimal NumberValue { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    public class DeleteSpecificationAttributeOptionDto
    {
        public Guid Id { get; set; }
    }
}