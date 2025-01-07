

namespace VFi.Application.PIM.DTOs
{
    public class ProductTopicPageDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Keywords { get; set; }
        public string? Slug { get; set; }
        public string? Image { get; set; }
        public string? Icon { get; set; }
        public string? Icon2 { get; set; }
        public string? Tags { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
    }
    public class ProductTopicPageSortDto
    {
        public Guid Id { get; set; }
        public int? SortOrder { get; set; }
    }
}
