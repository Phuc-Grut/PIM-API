

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ProductAttributeOptionSetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProductAttributeId { get; set; }
    }
}