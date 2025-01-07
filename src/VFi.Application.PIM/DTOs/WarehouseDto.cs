

namespace VFi.Application.PIM.DTOs
{
    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Company { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Api { get; set; }
        public string? Token { get; set; }
        public int? DisplayOrder { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedByName { get; set; }
        public int? Status { get; set; }
    }
    public class WarehouseListBoxDto
    {
        public Guid Value { get; set; }
        public string Label { get; set; }
        public string Key { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
    }
}
