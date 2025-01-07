using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddTaxCategoryRequest
    {
        public string? Code { get; set; }
        public string Name { get; set; }
        public double Rate { get; set; }
        public string? Group { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
        public int? Type { get; set; }
    }
    public class EditTaxCategoryRequest : AddTaxCategoryRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxTaxCategoryRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingTaxCategoryRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
