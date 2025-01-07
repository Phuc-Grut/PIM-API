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
    public class MercariRepository : SpiderClient, IMercariRepository
    {
        private readonly HttpClient _httpClientDefault;
        private readonly ILogger logger; 
        protected readonly IDictionary<string, string> defaultHeaders = new Dictionary<string, string>()
        {
            {"User-Agent", "Mozilla/5.0 {Windows NT 10.0; Win64; x64} AppleWebKit/537.36 {KHTML, like Gecko} Chrome/77.0.3865.90 Safari/537.36"},
            {"dpop", ""},
            {"x-platform", "web"},
            {"accept", "application/json, text/plain, */*"},
            {"accept-encoding", "gzip, deflate, br"},
            {"accept-language", "en,vi;q=0.9,en-US;q=0.8,ja;q=0.7"},
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
        public MercariRepository(IRequestManager requestManager, HttpClient httpClientDefault,
            ILogger<MercariRepository> logger) : base(requestManager,  requestManager.GetSpiderLeg())
        {
            this._httpClientDefault = httpClientDefault;
            this.logger = logger;
        }
   
        //--------------------------------------
       
        public async Task<string> GetItem(string item)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token; 
            var url = item;
            if (!item.StartsWith("https")) { 
                url = $"https://api.mercari.jp/items/get?id={item}&include_product_page_component=true"; 
            }
            else
            {
                var id = item.Split("/").ElementAt(4);
                url = $"https://api.mercari.jp/items/get?id={id}&include_product_page_component=true";
            }
            var currentToken = "";
            var content = await RetryWithNewProxy(async () =>
            {
                try
                { 
                    var token = requestManager.MercariToken.TokenItem;
                    currentToken = token;
                    defaultHeaders["dpop"] = token; 
                    return await GetStringAsync(url, HttpMethod.Get,null, configHttpRequestMessage: (requestMessage) =>
                        SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders));
                }
                catch (Exception ex)
                {
                    _ = ex;
                    return (httpResponse: HTTP_FAIL_CONTENT, "");
                }
            }, httpContent => HasNeedToRetry(httpContent.httpResponse, currentToken));


            return content.httpResponse;
        }
        public async Task<string> GetRelatedItems(string itemId)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
            var url = $"https://api.mercari.jp/items/related_items?item_id={itemId}&view_req_id=e14516663789daedd3b3c7b665c3bf21&limit=15";
            var currentToken = "";
            var content = await RetryWithNewProxy(async () =>
            {
                try
                {
                    var token = requestManager.MercariToken.TokenRelatedItems;
                    currentToken = token;
                    defaultHeaders["dpop"] = token;
                    return await GetStringAsync(url, HttpMethod.Get, null, configHttpRequestMessage: (requestMessage) =>
                        SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders));
                }
                catch (Exception ex)
                {
                    _ = ex;
                    return (httpResponse: HTTP_FAIL_CONTENT, "");
                }
            }, httpContent => HasNeedToRetry(httpContent.httpResponse, currentToken));


            return content.httpResponse;
        }
        public async Task<string> GetHomeItem(int take)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
            var url = $"https://api.mercari.jp/store/get_items?type=category&limit={take}";
            var currentToken = "";
            var content = await RetryWithNewProxy(async () =>
            {
                try
                {
                    var token = requestManager.MercariToken.TokenHome;
                    currentToken = token;
                    defaultHeaders["dpop"] = token;
                    return await GetStringAsync(url,HttpMethod.Get,null, configHttpRequestMessage: (requestMessage) =>
                        SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders));
                }
                catch (Exception ex)
                {
                    _ = ex;
                    return (httpResponse: HTTP_FAIL_CONTENT, "");
                }
            }, httpContent => HasNeedToRetry(httpContent.httpResponse, currentToken));

            return content.httpResponse;
        }
        public async Task<string> Filter(string? keyword, Dictionary<string, object> filter, int pageindex)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
            var url = "https://api.mercari.jp/v2/entities:search";
            var currentToken = "";
            int categoryId = 0;
            var pageToken = "";
            if (String.IsNullOrEmpty(keyword)) keyword = "";
            foreach (var item in filter)
            {
                if (item.Key.Equals("categoryId"))
                {
                    categoryId = int.Parse(item.Value+"");
                } 
            }
            if (pageindex > 0)
            {
                pageToken = "v1:" + pageindex;
            }
            var parameter = new {
                userId = "",
                pageSize = 120,
                pageToken = pageToken,
                searchSessionId = "99842c660343ed2f3e618d851b336322",
                indexRouting = "INDEX_ROUTING_UNSPECIFIED",
                searchCondition = new
                {
                    keyword=keyword,
                    excludeKeyword = "",
                    sort = "SORT_SCORE",
                    order = "ORDER_DESC",
                    categoryId = categoryId>0? new List<int>() { categoryId }: new List<int>(),
                    priceMin = 0,
                    priceMax = 0,
                    hasCoupon = false
                },
                defaultDatasets = new List<string>() { "DATASET_TYPE_MERCARI","DATASET_TYPE_BEYOND"},
                serviceFrom = "suruga",
                withItemBrand = true,
                withItemSize = false,
                withItemPromotions = false,
                withItemSizes = true,
                withShopname = false
            };
            var content = await RetryWithNewProxy(async () =>
            {
                try
                {
                    var token = requestManager.MercariToken.TokenSearch;
                    currentToken = token;
                    defaultHeaders["dpop"] = token;
                   var content =  new StringContent(Serialize.JsonSerializeObject(parameter), System.Text.Encoding.UTF8, "application/json");
                    return await GetStringAsync(url, HttpMethod.Post, content, configHttpRequestMessage: (requestMessage) =>
                      SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders)); 
                }
                catch (Exception ex)
                {
                    _ = ex;
                    return (httpResponse: HTTP_FAIL_CONTENT, "");
                }
            }, httpContent => HasNeedToRetry(httpContent.httpResponse, currentToken));

           

            return content.httpResponse;
        }

        public async Task<string> GetSellerItems(string sellerId,string status,int limit)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
            var url = $"https://api.mercari.jp/items/get_items?seller_id={sellerId}&limit={limit}&status={status}";
            var currentToken = "";
            var content = await RetryWithNewProxy(async () =>
            {
                try
                {
                    var token = requestManager.MercariToken.TokenSeller;
                    currentToken = token;
                    defaultHeaders["dpop"] = token;
                    return await GetStringAsync(url, HttpMethod.Get, null, configHttpRequestMessage: (requestMessage) =>
                        SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders));
                }
                catch (Exception ex)
                {
                    _ = ex;
                    return (httpResponse: HTTP_FAIL_CONTENT, "");
                }
            }, httpContent => HasNeedToRetry(httpContent.httpResponse, currentToken));


            return content.httpResponse;
        }
        public async Task<string> GetSellerInfo(string sellerId)
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(3)).Token;
            var url = $"https://api.mercari.jp/users/get_profile?user_id={sellerId}&_user_format=profile";
            var currentToken = "";
            var content = await RetryWithNewProxy(async () =>
            {
                try
                {
                    var token = requestManager.MercariToken.TokenSellerInfo;
                    currentToken = token;
                    defaultHeaders["dpop"] = token;
                    return await GetStringAsync(url, HttpMethod.Get, null, configHttpRequestMessage: (requestMessage) =>
                        SetHeaderForHttpRequestMessage(requestMessage, defaultHeaders));
                }
                catch (Exception ex)
                {
                    _ = ex;
                    return (httpResponse: HTTP_FAIL_CONTENT, "");
                }
            }, httpContent => HasNeedToRetry(httpContent.httpResponse, currentToken));


            return content.httpResponse;
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
