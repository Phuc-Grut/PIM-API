using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddDistrictRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? ShortName { get; set; }
        public string StateProvinceId { get; set; }
        public string? Note { get; set; }
    }
    public class EditDistrictRequest : AddDistrictRequest
    {
        public string Id { get; set; }
        public int Status { get; set; }
    }
    public class ListBoxDistrictRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$stateProvinceId")]
        public string? StateProvinceId { get; set; }
    }
    public class PagingDistrictRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$stateProvinceId")]
        public string? StateProvinceId { get; set; }
    }
}
