using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductMediaRepository : IRepository<ProductMedia>
    {
        Task<IEnumerable<ProductMedia>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductMedia>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        void Add(IEnumerable<ProductMedia> t);
    }
}
