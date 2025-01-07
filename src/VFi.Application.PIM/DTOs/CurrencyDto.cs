

namespace VFi.Application.PIM.DTOs
{
    public class CurrencyDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Locale { get; set; }
        public string? CustomFormatting { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
