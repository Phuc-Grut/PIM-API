

namespace VFi.Application.PIM.DTOs
{
    public class ProductTopicDetailDto
    {
        public Guid Id { get; set; }
        public string ProductTopic { get; set; }
        public string Code { get; set; } 
        public int Condition { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
        public string SourceLink { get; set; }
        public string SourceCode { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
        public string Images { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; }
        public Guid? CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Tags { get; set; }
        public DateTime? Exp { get; set; }
        public decimal? BidPrice { get; set; }
        public int? Tax { get; set; }
        public string Channel { get; set; }
        public decimal? ShippingFee { get; set; }
        public int? Bids { get; set; }
        public DateTime? PublishDate { get; set; }
    }
    public class ProductTopicDetailListBoxDto
    {
        public Guid Value { get; set; }
        public string Label { get; set; }
        public string Key { get; set; } 
    }
}
