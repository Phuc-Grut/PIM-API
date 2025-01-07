using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductReviewHelpfulnessRepository : IRepository<ProductReviewHelpfulness>
    {
        Task<IEnumerable<ProductReviewHelpfulness>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductReviewHelpfulness>> GetListListBox(Dictionary<string, object> filter);
    }
}
