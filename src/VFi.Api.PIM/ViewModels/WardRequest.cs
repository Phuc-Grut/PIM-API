using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
public class AddWardRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? ShortName { get; set; }
        public string DistrictId { get; set; }
        public string? Note { get; set; }
    }
    public class EditWardRequest : AddWardRequest
    {
        public string Id { get; set; }
        public int Status { get; set; }
    }
    public class ListBoxWardRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$districtId")]
        public string? DistrictId { get; set; }
    }
    public class PagingWardRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$districtId")]
        public string? DistrictId { get; set; }
    }
}
