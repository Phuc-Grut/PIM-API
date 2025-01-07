using Flurl;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Application.PIM.DTOs
{
    public class AuctionDto
    {
        public class Auction
        {
             
            public DateTime? StartTime { get; set; } 
            public DateTime? EndTime { get; set; } 
            public decimal? StartPrice { get; set; } 
            public decimal? HighestBid { get; set; }
            public decimal? BuyNowPrice { get; set; }
            public bool? IsAutomaticExtension { get; set; }
            public bool? IsEarlyClosing { get; set; }
            public int Bids { get; set; }
            public bool? IsClosed { get; set; }
            public bool AuctionEnded { get; set; }
        }
        public class AuctionProductPublish : ProductPublish
        {
            public AuctionProductPublish()
            {
            }
            public Auction Auction { get; set; } = new Auction();
            public string OriginCategoryId { get; set; }
            public string OriginCategory { get; set; }
            public string OriginCategoryIdPath { get; set; }
            public string OriginCategoryPath { get; set; }

            public bool IsFreeDomesticShipping { get; set; } = false;
            public decimal? DomesticShippingFee { get; set; }
            public int ConvertCondition(string aucCondition)
            {
                /// <summary>
                /// New = 0, Refurbished = 10,20 = LikeNew,  Used = 30, Damaged = 40
                /// </summary>
                ///  @"damaged|new|refurbished|used")
                switch (aucCondition)
                {
                    case "new":
                        return 0;
                    case "refurbished":
                        return 10;
                    case "used":
                        return 30;
                    case "damaged":
                        return 40;
                    default:
                        return 0;
                }
            }
        
            public void Convert(string content)
            {
               
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                var regexPid = Regex.Match(content, @"\SproductID\S\:\s\S(?<productId>[a-zA-Z0-9]+)\S,");
                if (regexPid.Success)
                {
                    this.Code = regexPid.Groups["productId"]?.Value;
                   
                }
                var url = $"https://page.auctions.yahoo.co.jp/jp/auction/{this.Code }";
                this.SourceLink = url;
                this.SourceCode = this.Code;
                this.Name = doc.QuerySelector(".ProductTitle__text")?.InnerText;
                this.FullDescription = doc.QuerySelector(".ProductExplanation__commentBody")?.InnerHtml;
                var rgSeoKeyword = Regex.Match(content, @"<meta\sname=""keywords""\scontent=""(?<metaKeyword>.*?)"">");
                if (rgSeoKeyword.Success)
                {
                    this.Tags = rgSeoKeyword.Groups["metaKeyword"]?.Value;
                }
                var rgDescription = Regex.Match(content, @"<meta\sname=""description""\scontent=""(?<metaDescription>.*?)"">");
                if (rgDescription.Success)
                {
                    this.ShortDescription = rgDescription.Groups["metaDescription"]?.Value;
                }
                #region ItemStatus 
                var regexItemStatus = Regex.Match(content, @"cndtn:\S(?<itemStatus>[a-zA-Z0-9._-]+)\',");
                if (regexItemStatus.Success)
                {
                    var itemStatus = regexItemStatus.Groups["itemStatus"]?.Value ?? string.Empty;
                    var rgItemCondition = Regex.Match(itemStatus.ToLower(), @"damaged|new|refurbished|used");
                    if (rgItemCondition.Success)
                    {
                        this.Condition = this.ConvertCondition(itemStatus);
                    }
                }
                #endregion ItemStatus
                this.Origin = "Japan";

                var regexQuantity = Regex.Match(content, @"num:\S(?<quantity>[0-9]+)\',");
                if (regexQuantity.Success)
                {
                    if (int.TryParse(regexQuantity.Groups["quantity"]?.Value, out var quantity))
                    {
                        this.StockQuantity = quantity;
                    }
                }
                this.IsStocking = true;

                var taxRateText = doc.QuerySelector(".Price__tax")?.InnerHtml?.Replace("（税", "")?.Replace("円）", "")?.Trim();
                if (taxRateText != null)
                {
                    this.TaxRate = taxRateText.Equals("0") ? 0 : 10;
                }
                var regex = new Regex(@"<script[^>]*>[\s\S]*?</script>");
                var jsonNews = regex.Matches(content).FirstOrDefault(x => x.Value.Contains("pageData"))?.Value ?? string.Empty;


                var regexShippingFee = Regex.Match(content, "data-chargeForShipping=\"(?<content>[0-9a-zA-Z]+)\"");
                if (regexShippingFee.Success)
                {
                    if (regexShippingFee.Groups["content"].Value == "0")
                    {
                        this.IsFreeDomesticShipping = true;
                    }
                }

                var regexCateId = Regex.Match(jsonNews, @"\SproductCategoryID\S\:\s\S(?<categoryId>[0-9]+)\S,");
                if (regexCateId.Success)
                {
                    this.OriginCategory = regexCateId.Groups["categoryId"]?.Value;
                    this.OriginCategoryId = regexCateId.Groups["categoryId"]?.Value;
                }
                var regexCategoryPath = Regex.Match(content, @"cat_path:(?<cat_path>(.+))\S");
                if (regexCategoryPath.Success)
                {
                    this.OriginCategoryIdPath = regexCategoryPath.Groups["cat_path"]?.Value;
                    this.OriginCategoryPath = regexCategoryPath.Groups["cat_path"]?.Value;
                }

                var nodeImages = doc.QuerySelectorAll(".ProductImage__images .ProductImage__inner img");
                var images = nodeImages.Select(x => x.Attributes["src"]?.Value)?.Where(x => !string.IsNullOrEmpty(x))?.ToList();
                if (images != null)
                {
                    int index = 1;
                    images.ForEach((image) =>
                    {
                        this.Images.Add(image ?? string.Empty);
                        index++;
                    });
                }
                if (this.Images.Any()) this.Image = this.Images.First();

                var itemReturnable = doc.QuerySelectorAll(".ProductDetail .l-right .ProductDetail__item:nth-child(1) .ProductDetail__description")?.FirstOrDefault()?.InnerText?.Trim();
                if (!string.IsNullOrEmpty(itemReturnable))
                {
                    itemReturnable = itemReturnable.Replace("：", string.Empty).Trim();
                    this.CanReturn = new[] { "あり", "返品可" }.Any(x => itemReturnable.StartsWith(x));
                }
                else
                {
                    this.CanReturn = false;
                }

                #region Price
                var regexPrice = Regex.Match(jsonNews, @"\Sprice\S\:\s\S(?<priceText>[0-9]+)\S,");
                if (regexPrice.Success)
                {
                    if (decimal.TryParse(regexPrice.Groups["priceText"]?.Value, out var price))
                    {
                        this.Auction.HighestBid = price;
                    }
                }
                #endregion

                #region StartTime
                var regexStartTime = Regex.Match(content, @"stm:\S(?<stm>[0-9]+)\',");
                if (regexStartTime.Success)
                {
                    if (long.TryParse(regexStartTime.Groups["stm"]?.Value, out var startTime))
                    {
                        this.Auction.StartTime = UnixTimeStampToDateTime(startTime);
                    }
                }
                #endregion StartTime

                #region EndTime
                var regexEndTime = Regex.Match(content, @"etm:\S(?<etm>[0-9]+)\',");
                if (regexEndTime.Success)
                {
                    if (long.TryParse(regexEndTime.Groups["etm"]?.Value, out var endTime))
                    {
                        this.Auction.EndTime = UnixTimeStampToDateTime(endTime);
                    }
                }
                #endregion EndTime

                #region Init Price
                var regexInitPrice = Regex.Match(content, @"spri:\S(?<initPrice>[a-zA-Z0-9._-|,]+)\',");
                if (long.TryParse(regexInitPrice.Groups["initPrice"]?.Value.Replace(",", ""), out var initPriceValue))
                {
                    this.Auction.StartPrice = initPriceValue;
                }
                #endregion Init Price

                #region Bids
                var regexBids = Regex.Match(jsonNews, @"\Sbids\S\:\s\S(?<bids>[0-9]+)\S,");
                if (regexBids.Success)
                {
                    if (int.TryParse(regexBids.Groups["bids"]?.Value, out var bids))
                    {
                        this.Auction.Bids = bids;
                    }
                }
                #endregion

                #region BuyNow
                var regexPriceBuyNow = Regex.Match(jsonNews, @"\SwinPrice\S\:\s\S(?<winPrice>[0-9]+)\S,");
                if (regexPriceBuyNow.Success)
                {
                    if (decimal.TryParse(regexPriceBuyNow.Groups["winPrice"]?.Value, out var priceBuyNow))
                    {
                        this.Price = priceBuyNow;
                        this.Auction.BuyNowPrice = priceBuyNow;
                    }
                }
                #endregion

                var isAutomaticExtension = doc.QuerySelectorAll("a[href='https://support.yahoo-net.jp/PccAuctions/s/article/H000008832']")?.FirstOrDefault()?.ParentNode.ParentNode.InnerText?.Trim();
                this.Auction.IsAutomaticExtension = !string.IsNullOrEmpty(isAutomaticExtension) && isAutomaticExtension.Contains("あり");

                var isEarlyClosing = doc.QuerySelectorAll("a[href='https://support.yahoo-net.jp/PccAuctions/s/article/H000005291']")?.FirstOrDefault()?.ParentNode.ParentNode.InnerText?.Trim();
                this.Auction.IsEarlyClosing = !string.IsNullOrEmpty(isEarlyClosing) && isEarlyClosing.Contains("あり");

                var nodebrand = doc.QuerySelectorAll("a[data-cl-params='_cl_vmodule:brand;_cl_link:lk;_cl_position:1;']")?.FirstOrDefault()?.InnerText?.Trim();
                this.Brand = nodebrand;

                var regexIsClosed = Regex.Match(jsonNews, @"\SisClosed\S\s\:\s\S(?<isClosed>[0-9]+)\S,");
                if (regexIsClosed.Success)
                {
                    this.Auction.IsClosed = !regexIsClosed.Groups["isClosed"]?.Value.Contains("0");
                }

                this.Auction.AuctionEnded = false;

            }

            //Utility
            private string? GetValueOfAttribute(HtmlNode node, string name)
            {
                return node?.Attributes?.FirstOrDefault(x => x.Name.Equals(name))?.Value?.Trim('\r', '\n', '\t');
            }

            private string? GetInnterTextBySelector(HtmlNode node, string selector)
            {
                return node?.QuerySelector(selector)?.InnerText?.Trim('\r', '\n', '\t');
            }

            private string? GetInnterTextBySelector(HtmlDocument doc, string selector)
            {
                return doc?.QuerySelector(selector)?.InnerText?.Trim('\r', '\n', '\t');
            }
            private int ValueIsNumber32(string str)
            {
                var value = Regex.Replace(str.Trim(), @"[^\d]", "");
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var isNumeric = int.TryParse(value, out int n);
                    return isNumeric ? n : 0;
                }
                return 0;
            }
            public DateTime UnixTimeStampToDateTime(long unixTimeStamp)
            {
                // Unix timestamp is seconds past epoch
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
                dateTime = dateTime.AddSeconds(unixTimeStamp).AddHours(7).AddSeconds(-3);
                return dateTime;
            }
        }
    }
}
