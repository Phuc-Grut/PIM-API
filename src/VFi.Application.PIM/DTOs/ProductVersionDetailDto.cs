

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ProductVersionDetailDto
    {
        public Guid Id { get; set; }
        public Guid? ProductVersionId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalMoney { get; set; }
        public string Note { set; get; }
    }
}
