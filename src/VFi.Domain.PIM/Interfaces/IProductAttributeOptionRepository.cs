using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductAttributeOptionRepository : IRepository<ProductAttributeOption>
    {
        Task<IEnumerable<ProductAttributeOption>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductAttributeOption>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        Task<IEnumerable<ProductAttributeOption>> Filter(Guid id);
        void Update(IEnumerable<ProductAttributeOption> items);
        void Add(IEnumerable<ProductAttributeOption> items);
        void Remove(IEnumerable<ProductAttributeOption> t);
        Task<IEnumerable<ProductAttributeOption>> GetByParentId(Guid id);
    }
}
