using VFi.Domain.PIM.Models;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductAttributeRequest
    {
        public string Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public bool AllowFiltering { get; set; }
        public int? SearchType { get; set; }
        public bool? IsOption { get; set; }
        public int DisplayOrder { get; set; }
        public string? Mapping { get; set; }
        public int? Status { get; set; }
        public List<AddProductAttributeOptionRequest>? Options { get; set; }
    }
    public class EditProductAttributeRequest : AddProductAttributeRequest
    {
        public string Id { get; set; }
    }

}
