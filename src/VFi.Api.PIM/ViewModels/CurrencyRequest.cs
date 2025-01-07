using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddCurrencyRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Locale { get; set; }
        public string? CustomFormatting { get; set; }
        public int Status { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditCurrencyRequest : AddCurrencyRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxCurrencyRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingCurrencyRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
}
