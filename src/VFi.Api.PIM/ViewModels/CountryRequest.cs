using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddCountryRequest
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? ShortName { get; set; }
        public string LocalName { get; set; } = null!;
        public string? Note { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditCountryRequest : AddCountryRequest
    {
        public string Id { get; set; } = null!;
        public int Status { get; set; }
    }

    public class ListBoxCountryRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }

    public class PagingCountryRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
