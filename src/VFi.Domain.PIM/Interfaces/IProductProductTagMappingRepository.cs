using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductProductTagMappingRepository : IRepository<ProductProductTagMapping>
    {
        Task<IEnumerable<ProductProductTagMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductProductTagMapping>> GetListListBox(Dictionary<string, object> filter);
        void Add(IEnumerable<ProductProductTagMapping> items);
        void Remove(IEnumerable<ProductProductTagMapping> t);
        Task<IEnumerable<ProductProductTagMapping>> Filter(Guid Id);
    }
}
