using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductReviewRepository : IRepository<ProductReview>
    {
        Task<IEnumerable<ProductReview>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductReview>> GetListListBox(Dictionary<string, object> filter, string? keyword);
    }
}
