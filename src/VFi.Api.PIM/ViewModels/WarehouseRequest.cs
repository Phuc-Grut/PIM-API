using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddWarehouseRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Company { get; set; }
        public string? Country { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Api { get; set; }
        public string? Token { get; set; }
        public int? DisplayOrder { get; set; }
        public int? Status { get; set; }
    }
    public class EditWarehouseRequest : AddWarehouseRequest
    {
        public string Id { get; set; }
    }
}
