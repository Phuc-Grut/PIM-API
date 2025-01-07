

using VFi.Domain.PIM.Models;

namespace VFi.Application.PIM.DTOs
{
    public class ExchangeRateDto
    {
        public Guid Id { get; set; }
        public Guid? CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
        public string? CurrencyCode { get; set; }
        public Decimal Rate { get; set; }
        public DateTime? ActiveDate { get; set; }
        public int Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
