using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models.Spider;
using VFi.Infra.PIM.Context;
using VFi.Infra.PIM.Spider;
using VFi.Infra.PIM.Spider.Interface;
using VFi.NetDevPack.Queries;
using Flurl.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VFi.Domain.PIM.Models.Spider.ProductListResponse;

namespace VFi.Infra.PIM.Repository
{
    public class SpiderRepository : SpiderClient, ISpiderRepository
    {
        private readonly SpiderApiContext _apiContext;
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
        protected IList<string> InvalidToken = new List<string> { };
        protected virtual bool HasNeedToRetry(string? content) => failContents.Any(item => string.IsNullOrEmpty(content) || content.Contains(item));
        //-----------------------------------------
        public SpiderRepository(IRequestManager requestManager, HttpClient httpClientDefault, SpiderApiContext apiContext,
            ILogger<SpiderRepository> logger) : base(requestManager, requestManager.GetSpiderLeg())
        {
            this._httpClientDefault = httpClientDefault;
            _apiContext = apiContext;
            this.logger = logger;
        }

        //--------------------------------------
        public async Task<string> Crawler(string url)
        {

            var content = await RetryWithNewProxy(async () =>
            {
                var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
                return await GetStringAsync(url, HttpMethod.Get, null,
                    configHttpRequestMessage: (requestMessage) => SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders),
                    cancellationToken: cancellationToken);
            }, htmlContent => HasNeedToRetry(htmlContent.httpResponse));

            return content.httpResponse;
        }
        public async Task<string> Crawler(string url, string? authorizationToken = null,
            string authorizationMethod = "Bearer")
        {

            var content = await RetryWithNewProxy(async () =>
            {
                var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
                return await GetStringAsync(url, HttpMethod.Get, null, authorizationToken, authorizationMethod,
                    configHttpRequestMessage: (requestMessage) => SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders),
                    cancellationToken: cancellationToken);
            }, htmlContent => HasNeedToRetry(htmlContent.httpResponse));

            return content.httpResponse;
        }

        //-----------------------------

        private const string PATH_SPIDER_SEARCH_AUCTION = "/api/auction/search";
        public async Task<ProductListResponse> AuctionSearch(string keyword, string category, string brandid, string seller, int? condition, int? sort, int pageSize = 20, int pageIndex = 1)
        {
            if (!condition.HasValue) condition = 0; if (!sort.HasValue) sort = 0;
            var result = await _apiContext.Client
          .Request(PATH_SPIDER_SEARCH_AUCTION)
          .SetQueryParam("query", keyword)
          .SetQueryParam("categoryCode", category)
          .SetQueryParam("seller", seller)
          .SetQueryParam("brand_id", brandid)
          .SetQueryParam("pageIndex", pageIndex)
          .SetQueryParam("pageSize", pageSize)
          .SetQueryParam("status", condition.Value)
          .SetQueryParam("sortId", sort.Value)
          .GetJsonAsync<ProductListResponse>();
            if (result != null)
            {
                return result;
            }
            return null;
        }
        private const string PATH_SPIDER_SEARCH_MERCARI = "/api/mercari/search";
        public async Task<ProductListResponse> MercariSearch(string keyword, string category, string brandid, string seller, int pageSize = 20, int pageIndex = 1)
        {
            var result = await _apiContext.Client
         .Request(PATH_SPIDER_SEARCH_MERCARI)
         .SetQueryParam("query", keyword)
         .SetQueryParam("categoryCode", category)
         .SetQueryParam("brand_id", brandid)
         .SetQueryParam("seller", seller)
         .SetQueryParam("pageIndex", pageIndex)
         .SetQueryParam("pageSize", pageSize)//.SetQueryParam("sortId", sort.Value)
         .GetJsonAsync<ProductListResponse>();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        private const string PATH_SPIDER_SEARCH_RAKUTEN= "/api/rakuten/search-items";
        public async Task<ProductListResponse> RakutenSearch(string keyword, string category, string brand, string seller, int pageIndex = 1)
        {
            var searchPara = new
            {
                Category = category,
                Keyword = keyword,
                Shop = seller,
                Page = pageIndex 
            };

            var result = await _apiContext.Client.WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
               .Request(PATH_SPIDER_SEARCH_RAKUTEN)
               .SetQueryParam("keyword", keyword)
                 .SetQueryParam("category", category)
                 .SetQueryParam("shop", seller)
                 .SetQueryParam("page", pageIndex)
                 .GetJsonAsync<ProductListResponse>();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        private const string PATH_SPIDER_SEARCH_GOLF = "/api/golfpartner/search-items";
        public async Task<ProductListResponse> GolfPartnerSearch(string keyword, string category, string searchType, string seller, int pageSize = 20, int pageIndex = 1)
        {
                        
            var obj = new
            {
                searchType = int.Parse(searchType),
                pageIndex = pageIndex,
                keyword = keyword,
                url = "",
                filterCondition = new object[]{
                     new {
                        name = "category",
                        value = category
                    }}

            };
            //var strPara = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            var result = await _apiContext.Client.WithHeader("Accept", "*/*").WithHeader("Content-Type", "application/json")
             .Request(PATH_SPIDER_SEARCH_GOLF)
             //.PostStringAsync(strPara)
             .PostJsonAsync(obj)
             //.PostStringAsync("{\r\n  \"searchType\": 1,\r\n  \"pageIndex\": 1,\r\n  \"keyword\": \"\",\r\n  \"url\": \"\",\r\n  \"filterCondition\": [\r\n    {\r\n      \"name\": \"category\",\r\n      \"value\": \"2010\"\r\n    }\r\n  ]\r\n}")
             .ReceiveJson<ProductListResponse>();

            if (result != null)
            {
                return result;
            }
            return null;
        }
    }
}
