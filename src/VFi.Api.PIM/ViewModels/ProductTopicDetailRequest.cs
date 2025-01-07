using VFi.Domain.PIM.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
  public class AddProductTopicDetailRequest
    {
        public string ProductTopic { get; set; }
        public string Code { get; set; }
        public int Condition { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }
        public string SourceLink { get; set; }
        public string SourceCode { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Origin { get; set; }
        public string Brand { get; set; }
        public string Manufacturer { get; set; }
        public string Image { get; set; }
        public string Images { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public int Status { get; set; } 
        public string Tags { get; set; }
        public DateTime? Exp { get; set; }
        public decimal? BidPrice { get; set; }
        public int? Tax { get; set; }
    }
    public class EditProductTopicDetailRequest : AddProductTopicDetailRequest
    {
        public string Id { get; set; }
    }
}
