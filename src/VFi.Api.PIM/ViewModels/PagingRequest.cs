using VFi.NetDevPack.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace VFi.Api.PIM.ViewModels
{
    public class PagingRequest
    {
        [FromQuery(Name = "$skip")]
        public int Skip { get; set; }
        [FromQuery(Name = "$top")]
        public int Top { get; set; }
        [FromQuery(Name = "$keyword")]
        public string? Keyword { get; set; } 
    }
    public class PagingRequestByTopic
    {
        [FromQuery(Name = "$skip")]
        public int Skip { get; set; }
        [FromQuery(Name = "$top")]
        public int Top { get; set; }
        [FromQuery(Name = "$keyword")]
        public string? Keyword { get; set; }
        [FromQuery(Name = "$topic")]
        public Guid? TopicId { get; set; }
        [FromQuery(Name = "channel")]
        public string? Channel { get; set; }
        [FromQuery(Name = "$sort")]
        public string? Sort { get; set; }
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
    }
    public class PagingRequestByPage
    {
        [FromQuery(Name = "$skip")]
        public int Skip { get; set; }
        [FromQuery(Name = "$top")]
        public int Top { get; set; }
        [FromQuery(Name = "$keyword")]
        public string? Keyword { get; set; }
        [FromQuery(Name = "$page")]
        public string? Page { get; set; }
        [FromQuery(Name = "channel")]
        public string? Channel { get; set; }
        [FromQuery(Name = "$status")]
        public int? Status { get; set; }
        [FromQuery(Name = "$sort")]
        public string? Sort { get; set; }
    }
    public class FilterInfor
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Ope { get; set; }
    }
    public class FilterQuery
    {
        public string? Filter { get; set; }

        public string? Order { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
        public string? Keyword { get; set; }
    }
}
