using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddUnitRequest
    {
        public AddUnitRequest()
        {
        }

        public string? Code { get; set; }
        public string Name { get; set; } = null!;
        public string NamePlural { get; set; } = null!;
        public string? Description { get; set; }
        public string? DisplayLocale { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; }
        public Guid GroupUnitId { get; set; }
        public double? Rate { get; set; }
        public int Status { get; set; }
    }
    public class EditUnitRequest 
    {
        public EditUnitRequest()
        {
        }

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string? Description { get; set; }
        public string? DisplayLocale { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsDefault { get; set; } 
        public Guid GroupUnitId { get; set; }
        public double? Rate { get; set; }
        public int Status { get; set; }
    }
    public class ListBoxUnitRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$groupUnitId")]
        public string? GroupUnitId { get; set; }

        [FromQuery(Name = "$nullAble")]
        public bool? NullAble { get; set; }
    }
    public class PagingUnitRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$groupUnitId")]
        public string? GroupUnitId { get; set; }
        [FromQuery(Name = "$groupUnitCode")]
        public string? GroupUnitCode { get; set; }
        [FromQuery(Name = "$Filter")]
        public string? Filter { get; set; }
    }
}
