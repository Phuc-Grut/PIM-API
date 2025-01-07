using VFi.Api.PIM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
    public class ListBoxRequest
    {
        [FromQuery(Name = "$keyword")]
        public string? Keyword { get; set; }
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
