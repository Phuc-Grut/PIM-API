using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddSpecificationAttributeOptionRequest
    {
        public Guid? Id { get; set; }
        public Guid SpecificationAttributeId { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public decimal NumberValue { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditSpecificationAttributeOptionRequest : AddSpecificationAttributeOptionRequest
    {
        public string Id { get; set; }
    }

    public class DeleteSpecificationAttributeOptionRequest
    {
        public Guid? Id { get; set; }
    }
    public class ListBoxSpecificationAttributeOptionRequest : ListBoxRequest
    {
        [FromQuery(Name = "$specificationAttributeId")]
        public string? SpecificationAttributeId { get; set; }
    }
    public class PagingSpecificationAttributeOptionRequest : PagingRequest
    {
        [FromQuery(Name = "$specificationAttributeId")]
        public string? SpecificationAttributeId { get; set; }
    }
}
