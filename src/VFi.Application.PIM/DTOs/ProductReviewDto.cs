

namespace VFi.Application.PIM.DTOs
{
    public class ProductReviewDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string? Title { get; set; }
        public string? ReviewText { get; set; }
        public int Rating { get; set; }
        public int HelpfulYesTotal { get; set; }
        public int HelpfulNoTotal { get; set; }
        public bool? IsVerifiedPurchase { get; set; }
    }
    public class ProductReviewQueryParams
    {
        public string? ProductId { get; set; }
    }
}
