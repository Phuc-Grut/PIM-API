using VFi.Domain.PIM.Models.Spider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Interfaces
{
    public interface ISpiderRepository
    {
        Task<string> Crawler(string url);

        Task<string> Crawler(string url, string? authorizationToken = null, string authorizationMethod = "Bearer");
        Task<ProductListResponse> AuctionSearch(string keyword, string category,string brandid, string seller, int? condition, int? sort, int pageSize = 20, int pageIndex = 1);
        Task<ProductListResponse> MercariSearch(string keyword, string category, string brandid, string seller, int pageSize = 20, int pageIndex = 1);
        Task<ProductListResponse> RakutenSearch(string keyword, string category, string brand, string seller, int pageIndex = 1);
        Task<ProductListResponse> GolfPartnerSearch(string keyword, string category, string searchType, string seller, int pageSize = 20, int pageIndex = 1);
    }
}
