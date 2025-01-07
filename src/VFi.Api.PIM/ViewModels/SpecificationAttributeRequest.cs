using VFi.Domain.PIM.Models;

namespace VFi.Api.PIM.ViewModels
{
  public class AddSpecificationAttributeRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public List<AddSpecificationAttributeOptionRequest>? Options { get; set; }
    }
    public class EditSpecificationAttributeRequest : AddSpecificationAttributeRequest
    {
        public string Id { get; set; }
    }

}
