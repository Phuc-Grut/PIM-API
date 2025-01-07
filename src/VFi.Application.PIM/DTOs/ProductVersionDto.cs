

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ProductVersionDto
    {
        public Guid Id { get; set; }
        public Guid? ProductId { get; set; }
        public string Code { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedByName { set; get; }
    }
}
