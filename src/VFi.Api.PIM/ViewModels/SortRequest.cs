using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
    public class Sort
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }
    }
    public class SortRequest
    {
        public List<Sort> SortList { get; set; } = null!;
    }
}
