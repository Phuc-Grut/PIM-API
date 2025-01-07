using VFi.Domain.PIM.Models;

namespace VFi.Api.PIM.ViewModels
{
  public class AddStoreRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditStoreRequest : AddStoreRequest
    {
        public string Id { get; set; }
    }

}
