using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductServiceAddRepository : IRepository<ProductServiceAdd>
    {
        Task<IEnumerable<ProductServiceAdd>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductServiceAdd>> GetListListBox(Dictionary<string, object> filter);
        Task<IEnumerable<ProductServiceAdd>> Filter(Guid Id);
        void Add(IEnumerable<ProductServiceAdd> items);
        void Remove(IEnumerable<ProductServiceAdd> t);
        void Update(IEnumerable<ProductServiceAdd> t);
        Task<IEnumerable<ProductServiceAdd>> Filter(Dictionary<string, object> filter);
        Task<IEnumerable<ProductServiceAdd>> GetByParentId(Guid id);
    }
}
