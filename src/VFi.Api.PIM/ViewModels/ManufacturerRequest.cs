using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddManufacturerRequest
    {
        public string? Code { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditManufacturerRequest : AddManufacturerRequest
    {
        public string Id { get; set; } = null!;
    }
    public class ListBoxManufacturerRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingManufacturerRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
