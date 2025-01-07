using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductStoreMappingRepository : IRepository<ProductStoreMapping>
    {
        Task<IEnumerable<ProductStoreMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductStoreMapping>> GetListListBox(Dictionary<string, object> filter);

        Task<IEnumerable<ProductStoreMapping>> Filter(Guid Id);

        void Add(IEnumerable<ProductStoreMapping> items);

        void Remove(IEnumerable<ProductStoreMapping> t);
    }
}
