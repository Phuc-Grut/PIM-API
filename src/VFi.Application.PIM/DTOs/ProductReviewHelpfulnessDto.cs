

namespace VFi.Application.PIM.DTOs
{
    public class ProductReviewHelpfulnessDto
    {
        public Guid Id { get; set; }
        public Guid ProductReviewId { get; set; }
        public bool WasHelpful { get; set; }
    }
    public class ProductReviewHelpfulnessQueryParams
    {
        public string? ProductReviewId { get; set; }
    }
}
