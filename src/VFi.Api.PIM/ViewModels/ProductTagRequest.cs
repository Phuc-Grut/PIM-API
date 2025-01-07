using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductTagRequest
    {
        public string Name { get; set; }
        public int Status { get; set; }
        public int? Type { get; set; }
    }
    public class EditProductTagRequest : AddProductTagRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxProductTagRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$type")]
        public int? Type { get; set; }
    }
    public class PagingProductTagRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
