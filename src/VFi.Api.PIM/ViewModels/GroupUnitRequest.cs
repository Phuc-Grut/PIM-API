using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddGroupUnitRequest
    {
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditGroupUnitRequest : AddGroupUnitRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxGroupUnitRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingGroupUnitRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$Filter")]
        public string? Filter { get; set; }
    }
}
