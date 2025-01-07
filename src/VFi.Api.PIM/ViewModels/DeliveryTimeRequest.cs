using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddDeliveryTimeRequest
    {
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
        public int? MinDays { get; set; }
        public int? MaxDays { get; set; }
        public int? Status { get; set; }
        public int DisplayOrder { get; set; }
    }
    public class EditDeliveryTimeRequest : AddDeliveryTimeRequest
    {
        public string Id { get; set; }
    }

}
