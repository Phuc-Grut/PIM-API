using VFi.Api.PIM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
    public class AddProductTypeRequest
    {
        public string? Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditProductTypeRequest : AddProductTypeRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxProductTypeRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingProductTypeRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
