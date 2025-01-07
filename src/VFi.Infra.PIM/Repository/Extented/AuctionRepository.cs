using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Spider;
using VFi.Infra.PIM.Spider.Interface;
using VFi.NetDevPack.Serialize;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VFi.Infra.PIM.Repository
{
    public class AuctionRepository : SpiderClient, IAuctionRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly HttpClient _httpClientDefault;
        private readonly ILogger logger;
        private readonly IDictionary<string, string> defaultHeaders = new Dictionary<string, string>()
        {
            {"User-Agent", "Mozilla/5.0 {Windows NT 10.0; Win64; x64} AppleWebKit/537.36 {KHTML, like Gecko} Chrome/77.0.3865.90 Safari/537.36"},
            {"Accept-Charset", "ISO-8859-1"},
            {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9"},
            {"accept-encoding", "gzip, deflate, br"}
        };
        protected const string HTTP_FAIL_CONTENT = "___HTTP_FAIL___";
        private readonly string[] failContents = new string[]
        {
            HTTP_FAIL_CONTENT,
            "The VietPN Software Foundation and contributors",
            "unauthorized: invalid token",
            "unauthorized: missing auth token"
        }; 
        //-----------------------------------------
        public AuctionRepository(IRequestManager requestManager, HttpClient httpClientDefault, IDistributedCache distributedCache,
            ILogger<AuctionRepository> logger) : base(requestManager,  requestManager.GetSpiderLeg())
        {
            this._httpClientDefault = httpClientDefault; _distributedCache = distributedCache;
            this.logger = logger;
        }
   
        //--------------------------------------
       
        public async Task<string> GetProductDetail(string item)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
            // var cached_key = $"AUC_DETAIL_{itemId}";
            // string cachedString = string.Empty;
            //cachedString = await _distributedCache.GetStringAsync(cached_key);
            //if (!string.IsNullOrEmpty(cachedString)) return cachedString;
            var url = item;
            if (!item.StartsWith("https")) url = $"https://page.auctions.yahoo.co.jp/jp/auction/{item}";
            var content = await RetryWithNewProxy(async () =>
            {
                try
                { 
                    return await GetStringAsync(url, HttpMethod.Get,null, configHttpRequestMessage: (requestMessage) =>
                        SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders));
                }
                catch (Exception ex)
                {
                    _ = ex;
                    return (httpResponse: HTTP_FAIL_CONTENT, "");
                }
            }, httpContent => HasNeedToRetry(httpContent.httpResponse));

           // var options = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3));
           // await _distributedCache.SetStringAsync(cached_key, content.httpResponse, options);

            return content.httpResponse;
        }
        public async Task<string> GetRelatedItems(string itemId)
        {
            return "";
        }
        public async Task<string> GetHomeItem(int take)
        {
           
            return "";
        }
        public async Task<string> Filter(string? keyword, Dictionary<string, object> filter, int pageindex)
        {
            return "";
        }

        public async Task<string> GetSellerItems(string sellerId,string status,int limit)
        {
            return "";
        }
        public async Task<string> GetSellerInfo(string sellerId)
        {
            return "";
        }
        //-----------------------------
        protected virtual bool HasNeedToRetry(string? content, string? curentToken = "")
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(curentToken) && (content.Contains("unauthorized: invalid token")))
            {
                requestManager.MercariToken.TokenClear(curentToken);
            }

            return failContents.Any(item => content.Contains(item));
        }
        protected virtual bool TokenNeedToRetry(object? response, string? curentToken = "")
        {
            if (response != null)
            {
                return false;
            }

            requestManager.MercariToken.TokenClear(curentToken);
            return true;
        }

        
    }
}
