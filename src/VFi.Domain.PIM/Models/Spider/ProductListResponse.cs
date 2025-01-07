using VFi.NetDevPack.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Models.Spider
{
    public class ProductListResponse : PagingResponse<ProductListResponse.ProductListItem>
    {
        public IList<FilterCondition>? FilterConditions { get; set; }

        public IList<FilterConditionItem> Sorts { get; set; }

        public class FilterCondition
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
            public IList<FilterConditionItem> Items { get; set; }
        }

        public class FilterConditionItem
        {
            public string QueryParams { get; set; }
            public string? Name { get; set; }
            public string Id { get; set; }
            public string Types { get; set; }
            public string From { get; set; }
            public bool IsChecked { get; set; }
        }

        public class ProductListItem
        {
            public string ProductSource { get; set; }
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public string ProductLink { get; set; }
            public string PreviewImage { get; set; }
            public string? SellerId { get; set; }
            public string? CategoryId { get; set; }

            public string Route { get; set; }
            public string Currency { get; set; }
            public decimal Price { get; set; }
            public decimal? BuyNowPrice { get; set; }
            public int? Tax { get; set; }

            /// <summary>
            /// ProductStatus.cs
            /// </summary>
            public int Status { get; set; } = 0;

            public decimal? ShippingFee { get; set; }
            public IList<string>? Images { get; set; } = new List<string>();
            public int? Bids { get; set; }
            public DateTime? EndTime { get; set; }
            public bool? IsFreeDelivery { get; set; }

            public ProductListItem SetStatus(int status)
            {
                Status = status;
                return this;
            } 
        }
    }
}
