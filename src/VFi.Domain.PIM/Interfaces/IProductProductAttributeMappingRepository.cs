using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductProductAttributeMappingRepository : IRepository<ProductProductAttributeMapping>
    {
        Task<IEnumerable<ProductProductAttributeMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductProductAttributeMapping>> GetListListBox(Dictionary<string, object> filter);
        void Add(IEnumerable<ProductProductAttributeMapping> items);
        Task<IEnumerable<ProductProductAttributeMapping>> Filter(Guid Id);
        void Remove(IEnumerable<ProductProductAttributeMapping> t);
        Task<IEnumerable<ProductProductAttributeMapping>> Filter(Dictionary<string, object> filter);
    }
}
