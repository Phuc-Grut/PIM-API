

namespace VFi.Application.PIM.DTOs
{
    public class RelatedProductDto
    {
        public Guid? Id { get; set; }
        public Guid ProductId1 { get; set; }
        public Guid ProductId2 { get; set; }
        public int DisplayOrder { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Image { get; set; }

    }

    public class RelatedProductQueryParams
    {
        public string? ProductId1 { get; set; }
        public string? ProductId2 { get; set; }
      
    }
}
