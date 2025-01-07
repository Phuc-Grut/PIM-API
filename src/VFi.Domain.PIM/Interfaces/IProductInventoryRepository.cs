using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductInventoryRepository : IRepository<ProductInventory>
    {
        Task<IEnumerable<ProductInventory>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductInventory>> GetListListBox(Dictionary<string, object> filter);
        Task<IEnumerable<ProductInventory>> Filter(Guid Id);
        Task<IEnumerable<ProductInventory>> GetByParentId(Guid Id);
        Task<IEnumerable<ProductInventory>> Filter(IEnumerable<Product> products);
        void Add(IEnumerable<ProductInventory> items);

        void Remove(IEnumerable<ProductInventory> t);
        void Update(IEnumerable<ProductInventory> t);
    }
}
