using VFi.Domain.PIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Interfaces
{
    public interface IMercariRepository
    { 
        Task<string> GetItem(string url);
        Task<string> GetRelatedItems(string itemId);
        Task<string> GetHomeItem(int take);
        Task<string> Filter(string? keyword, Dictionary<string, object> filter, int pageindex);
        Task<string> GetSellerItems(string sellerId, string status, int limit);
        Task<string> GetSellerInfo(string sellerId);
    }
}
