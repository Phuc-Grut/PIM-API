using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
 public class AddStateProvinceRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? ShortName { get; set; }
        public string CountryId { get; set; }
        public string? Note { get; set; }
    }
    public class EditStateProvinceRequest : AddStateProvinceRequest
    {
        public string Id { get; set; }
        public int Status { get; set; }
    }
    public class ListBoxStateProvinceRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$countryId")]
        public string? CountryId { get; set; }
    }
    public class PagingStateProvinceRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$countryId")]
        public string? CountryId { get; set; }
    }
}
