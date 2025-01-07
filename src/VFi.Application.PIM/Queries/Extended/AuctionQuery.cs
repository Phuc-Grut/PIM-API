using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Flurl;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static VFi.Application.PIM.DTOs.AuctionDto;
using static MassTransit.ValidationResultExtensions;

namespace VFi.Application.PIM.Queries
{
    public class AuctionItemQuery : IQuery<AuctionProductPublish>
    {
        public AuctionItemQuery(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
    public class AuctionRelatedItemsQuery : IQuery<List<ProductListView>>
    {
        public AuctionRelatedItemsQuery(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; set; }
    }
    public class AuctionQueryHandler : IQueryHandler<AuctionItemQuery, AuctionProductPublish>, IQueryHandler<AuctionRelatedItemsQuery, List<ProductListView>>
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionQueryHandler(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }



        public async Task<AuctionProductPublish> Handle(AuctionItemQuery request, CancellationToken cancellationToken)
        {
            var url = $"https://page.auctions.yahoo.co.jp/jp/auction/{request.ItemId}";
            var result = new AuctionProductPublish();
            var content = await _auctionRepository.GetProductDetail(request.ItemId);

            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var regexPid = Regex.Match(content, @"\SproductID\S\:\s\S(?<productId>[a-zA-Z0-9]+)\S,");
            if (regexPid.Success)
            {
                result.Code = regexPid.Groups["productId"]?.Value;
                result.SourceLink = url;
                result.SourceCode = result.Code;
            }
            result.Name = doc.QuerySelector(".ProductTitle__text")?.InnerText;
            result.FullDescription = doc.QuerySelector(".ProductExplanation__commentBody")?.InnerHtml; 
            var rgSeoKeyword = Regex.Match(content, @"<meta\sname=""keywords""\scontent=""(?<metaKeyword>.*?)"">");
            if (rgSeoKeyword.Success)
            {
                result.Tags = rgSeoKeyword.Groups["metaKeyword"]?.Value;
            }
            var rgDescription = Regex.Match(content, @"<meta\sname=""description""\scontent=""(?<metaDescription>.*?)"">");
            if (rgDescription.Success)
            {
                result.ShortDescription = rgDescription.Groups["metaDescription"]?.Value;
            } 
            #region ItemStatus 
            var regexItemStatus = Regex.Match(content, @"cndtn:\S(?<itemStatus>[a-zA-Z0-9._-]+)\',");
            if (regexItemStatus.Success)
            {
                var itemStatus = regexItemStatus.Groups["itemStatus"]?.Value ?? string.Empty; 
                var rgItemCondition = Regex.Match(itemStatus.ToLower(), @"damaged|new|refurbished|used");
                if (rgItemCondition.Success)
                {
                    result.Condition = result.ConvertCondition(itemStatus);
                }  
            }
            #endregion ItemStatus
            result.Origin = "Japan";

            var regexQuantity = Regex.Match(content, @"num:\S(?<quantity>[0-9]+)\',");
            if (regexQuantity.Success)
            {
                if (int.TryParse(regexQuantity.Groups["quantity"]?.Value, out var quantity))
                {
                    result.StockQuantity = quantity;
                }
            }
            result.IsStocking= true;

            var taxRateText = doc.QuerySelector(".Price__tax")?.InnerHtml?.Replace("（税", "")?.Replace("円）", "")?.Trim();
            if (taxRateText != null)
            {
                result.TaxRate = taxRateText.Equals("0") ? 0 : 10;
            }
            var regex = new Regex(@"<script[^>]*>[\s\S]*?</script>");
            var jsonNews = regex.Matches(content).FirstOrDefault(x => x.Value.Contains("pageData"))?.Value ?? string.Empty;
          
             
            var regexShippingFee = Regex.Match(content, "data-chargeForShipping=\"(?<content>[0-9a-zA-Z]+)\"");
            if (regexShippingFee.Success)
            {
                if (regexShippingFee.Groups["content"].Value == "0")
                {
                    result.IsFreeDomesticShipping = true;
                }
            }

            var regexCateId = Regex.Match(jsonNews, @"\SproductCategoryID\S\:\s\S(?<categoryId>[0-9]+)\S,");
            if (regexCateId.Success)
            {
                result.OriginCategory = regexCateId.Groups["categoryId"]?.Value;
                result.OriginCategoryId = regexCateId.Groups["categoryId"]?.Value; 
            }
            var regexCategoryPath = Regex.Match(content, @"cat_path:(?<cat_path>(.+))\S");
            if (regexCategoryPath.Success)
            {
                result.OriginCategoryIdPath = regexCategoryPath.Groups["cat_path"]?.Value;
                result.OriginCategoryPath = regexCategoryPath.Groups["cat_path"]?.Value;
            }

            var nodeImages = doc.QuerySelectorAll(".ProductImage__images .ProductImage__inner img");
            var images = nodeImages.Select(x => x.Attributes["src"]?.Value)?.Where(x => !string.IsNullOrEmpty(x))?.ToList(); 
            if (images != null)
            {
                int index = 1;
                images.ForEach((image) =>
                {
                    result.Images.Add(image ?? string.Empty);
                    index++;
                });
            }
            if(result.Images.Any()) result.Image= result.Images.First();

            var itemReturnable =  doc.QuerySelectorAll(".ProductDetail .l-right .ProductDetail__item:nth-child(1) .ProductDetail__description")?.FirstOrDefault()?.InnerText?.Trim();
            if (!string.IsNullOrEmpty(itemReturnable))
            {
                itemReturnable = itemReturnable.Replace("：", string.Empty).Trim(); 
                result.CanReturn = new[] { "あり", "返品可" }.Any(x => itemReturnable.StartsWith(x));
            }
            else
            {
                result.CanReturn = false;
            }

            #region Price
            var regexPrice = Regex.Match(jsonNews, @"\Sprice\S\:\s\S(?<priceText>[0-9]+)\S,");
            if (regexPrice.Success)
            {
                if (decimal.TryParse(regexPrice.Groups["priceText"]?.Value, out var price))
                { 
                    result.Auction.HighestBid = price;
                }
            } 
            #endregion

            #region StartTime
            var regexStartTime = Regex.Match(content, @"stm:\S(?<stm>[0-9]+)\',");
            if (regexStartTime.Success)
            {
                if (long.TryParse(regexStartTime.Groups["stm"]?.Value, out var startTime))
                {
                    result.Auction.StartTime = UnixTimeStampToDateTime(startTime);
                }
            }
            #endregion StartTime

            #region EndTime
            var regexEndTime = Regex.Match(content, @"etm:\S(?<etm>[0-9]+)\',");
            if (regexEndTime.Success)
            {
                if (long.TryParse(regexEndTime.Groups["etm"]?.Value, out var endTime))
                {
                    result.Auction.EndTime = UnixTimeStampToDateTime(endTime);
                }
            }
            #endregion EndTime

            #region Init Price
            var regexInitPrice = Regex.Match(content, @"spri:\S(?<initPrice>[a-zA-Z0-9._-|,]+)\',");
            if (long.TryParse(regexInitPrice.Groups["initPrice"]?.Value.Replace(",", ""), out var initPriceValue))
            {
                result.Auction.StartPrice = initPriceValue;
            }
            #endregion Init Price

            #region Bids
            var regexBids = Regex.Match(jsonNews, @"\Sbids\S\:\s\S(?<bids>[0-9]+)\S,");
            if (regexBids.Success)
            {
                if (int.TryParse(regexBids.Groups["bids"]?.Value, out var bids))
                {
                    result.Auction.Bids = bids;
                }
            }
            #endregion

            #region BuyNow
            var regexPriceBuyNow = Regex.Match(jsonNews, @"\SwinPrice\S\:\s\S(?<winPrice>[0-9]+)\S,");
            if (regexPriceBuyNow.Success)
            {
                if (decimal.TryParse(regexPriceBuyNow.Groups["winPrice"]?.Value, out var priceBuyNow))
                {
                    result.Price = priceBuyNow;
                    result.Auction.BuyNowPrice = priceBuyNow;
                }
            }
            #endregion

            var isAutomaticExtension = doc.QuerySelectorAll("a[href='https://support.yahoo-net.jp/PccAuctions/s/article/H000008832']")?.FirstOrDefault()?.ParentNode.ParentNode.InnerText?.Trim();
            result.Auction.IsAutomaticExtension = !string.IsNullOrEmpty(isAutomaticExtension) && isAutomaticExtension.Contains("あり");

            var isEarlyClosing =   doc.QuerySelectorAll("a[href='https://support.yahoo-net.jp/PccAuctions/s/article/H000005291']")?.FirstOrDefault()?.ParentNode.ParentNode.InnerText?.Trim();
            result.Auction.IsEarlyClosing = !string.IsNullOrEmpty(isEarlyClosing) && isEarlyClosing.Contains("あり");

            var nodebrand = doc.QuerySelectorAll("a[data-cl-params='_cl_vmodule:brand;_cl_link:lk;_cl_position:1;']")?.FirstOrDefault()?.InnerText?.Trim();
            result.Brand = nodebrand;

            var regexIsClosed = Regex.Match(jsonNews, @"\SisClosed\S\s\:\s\S(?<isClosed>[0-9]+)\S,");
            if (regexIsClosed.Success)
            {
                result.Auction.IsClosed = !regexIsClosed.Groups["isClosed"]?.Value.Contains("0"); 
            }

            result.Auction.AuctionEnded = false;

            return result;
        }

        public async Task<List<ProductListView>> Handle(AuctionRelatedItemsQuery request, CancellationToken cancellationToken)
        {
            var url = $"https://page.auctions.yahoo.co.jp/jp/auction/{request.ItemId}";
            var result = new List<ProductListView>();
            var content = await _auctionRepository.GetProductDetail(request.ItemId);
            var doc = new HtmlDocument();
            doc.LoadHtml(content);

            var nodeRecommentTop = doc.QuerySelector("#recommendTop");
            if (nodeRecommentTop != null)
            {
                var products = nodeRecommentTop.QuerySelectorAll("ul li");
                if (products != null && products.Count > 0)
                { 
                    foreach (var item in products)
                    {
                        var model = RelatedProductItem(item);
                        if (model != null)
                        {
                            result.Add(model);
                        }
                    }
                }
            }

            var nodeRecommentBottom = doc.QuerySelector("#recommendBottom");
            if (nodeRecommentBottom != null)
            {
                var products = nodeRecommentBottom.QuerySelectorAll("ul li");
                if (products != null && products.Count > 0)
                { 
                    foreach (var item in products)
                    {
                        var model = RelatedProductItem(item);
                        if (model != null)
                        {
                            result.Add(model);
                        }
                    }
                }
            }

            if (result.Any())
            {
                return result;
            }

            // hàm cũ ko có kết quả trả về
            var body = doc.DocumentNode.Descendants("div")
               .Where(node => node.GetAttributeValue("class", "").Equals("Carousel__body js-carousel-body"))?.FirstOrDefault();
            if (body != null)
            {
                result = GetContentRelatedProduct(body); 
                return result;
            }
            body = doc.DocumentNode.Descendants("div")
              .Where(node => node.GetAttributeValue("class", "").Equals("Carousel Carousel--patA js-carousel-patA"))?.FirstOrDefault();
            if (body != null)
            {
                result  = GetContentRelatedProduct(body); 
                return result;
            }
            body = doc.DocumentNode.Descendants("div")
              .Where(node => node.GetAttributeValue("class", "").Equals("CarouselTest__inner js-carouselTest-view"))?.FirstOrDefault();
            if (body != null)
            {
                result = GetContentRelatedProduct(body); 
                return result;
            }

            return result;
        }
        private ProductListView? RelatedProductItem(HtmlNode node)
        {
            //var url = $"https://page.auctions.yahoo.co.jp/jp/auction/{request.ItemId}"; result.SourceLink = url;
            try
            {
                var nodeId = node.QuerySelector(".WatchButton");
                var id = GetValueOfAttribute(nodeId, "data-aid"); //node.SelectSingleNode(".//[@class='WatchButton']")?.Attributes["data-aid"]?.Value.Trim('\r', '\n', '\t');
                var nodeImg = node.QuerySelector(".ProductItem__imageData");
                var title = GetValueOfAttribute(nodeImg, "alt");
                var image = GetValueOfAttribute(nodeImg, "src");
                var model = new ProductListView
                {
                    Name = title,
                    Code = id,
                    SourceLink= $"https://page.auctions.yahoo.co.jp/jp/auction/{id}",
                    SourceCode = id,
                    Image = image
                };
                var price = GetInnterTextBySelector(node, ".ProductItem__priceValue--current");
                if (!string.IsNullOrEmpty(price))
                {
                    model.Price = ValueIsNumber32(price); 
                    model.Currency = "JPY";
                }
                model.Origin = "Japan";
                model.IsStocking = true;
                return model;
            }
            catch (Exception ex)
            {
                 
            }
            return null;
        }

        private List<ProductListView>? GetContentRelatedProduct(HtmlNode node)
        {
            try
            {
                var items = node.Descendants("li").Select(m =>
                {
                    var priceDisplay = m.Descendants("em").Where(p => p.GetAttributeValue("class", "").Equals("Carousel__count"))
                    .FirstOrDefault()?.InnerText.Replace(",", "") ?? string.Empty;
                    var price = int.Parse(Regex.Match(priceDisplay, @"([\d]+)").Groups[1].Value);
                    var title = m.Descendants("span").Where(p => p.GetAttributeValue("class", "").Equals("Carousel__name"))
                    .FirstOrDefault()?.InnerText ?? string.Empty;
                    var urlImage = m.SelectNodes("a/span/img").FirstOrDefault()?.Attributes["src"].Value ?? string.Empty;
                    var Id = m.SelectNodes("a").FirstOrDefault()?.GetAttributeValue("href", "").Split('/').LastOrDefault() ?? string.Empty;
                    var model = new ProductListView()
                    {
                        Code = Id,
                        Price = price,
                        SourceLink = $"https://page.auctions.yahoo.co.jp/jp/auction/{Id}",
                        SourceCode  = Id,
                        Name = title,
                        Image = urlImage,
                    };
                    model.Currency = "JPY";
                    model.Origin = "Japan";
                    model.IsStocking = true;
                    return model;
                }).ToList();

                return items;
            }
            catch (Exception ex)
            { 
            }

            try
            {
                var items = node.Descendants("li").Select(m =>
                {
                    var priceDisplay = m.Descendants("span").Where(p => p.GetAttributeValue("class", "").Equals("ProductItem__priceValue ProductItem__priceValue--current"))
                    .FirstOrDefault()?.InnerText.Replace(",", "").Replace("円", "") ?? string.Empty;
                    var price = int.Parse(Regex.Match(priceDisplay, @"([\d]+)").Groups[1].Value);
                    var title = m.Descendants("p").Where(p => p.GetAttributeValue("class", "").Equals("ProductItem__title"))
                    .FirstOrDefault()?.InnerText;
                    var urlImage = m.Descendants("img").Where(p => p.GetAttributeValue("class", "").Equals("ProductItem__imageData"))
                    .FirstOrDefault()?.Attributes["src"].Value;
                    var Id = m.Descendants("a").Where(p => p.GetAttributeValue("class", "").Equals("ProductItem__link rapidnofollow"))
                    .FirstOrDefault()?.GetAttributeValue("href", "").Split('/').LastOrDefault();
                    var model = new ProductListView()
                    {
                        Code = Id,
                        Price = price,
                        SourceLink = $"https://page.auctions.yahoo.co.jp/jp/auction/{Id}",
                        SourceCode = Id,
                        Name = title,
                        Image = urlImage,
                    };
                    model.Currency = "JPY";
                    model.Origin = "Japan";
                    model.IsStocking = true;
                    return model;
                }).ToList();
                return items;
            }
            catch (Exception ex)
            { 
            }
            return null;
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
