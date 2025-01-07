using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddExchangeRateRequest
    {
        public string CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public Decimal Rate { get; set; }
        public DateTime ActiveDate { get; set; }
        public int Status { get; set; }
    }
    public class EditExchangeRateRequest : AddExchangeRateRequest
    {
        public string Id { get; set; }
    }
    public class ListBoxExchangeRateRequest : ListBoxRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }

        [FromQuery(Name = "$currencyId")]
        public string? CurrencyId { get; set; }
    }
    public class PagingExchangeRateRequest : PagingRequest
    {
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$currencyId")]
        public string? CurrencyId { get; set; }
    }
}
