using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductWarehouseRepository : IRepository<ProductWarehouse>
    {
        Task<IEnumerable<ProductWarehouse>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductWarehouse>> GetListListBox(Dictionary<string, object> filter);
        void Add(IEnumerable<ProductWarehouse> items);
        Task<IEnumerable<ProductWarehouse>> Filter(Guid Id);
        void Remove(IEnumerable<ProductWarehouse> t);
    }
}
