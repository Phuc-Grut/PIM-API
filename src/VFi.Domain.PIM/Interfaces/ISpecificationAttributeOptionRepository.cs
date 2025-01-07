using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ISpecificationAttributeOptionRepository : IRepository<SpecificationAttributeOption>
    {
        Task<IEnumerable<SpecificationAttributeOption>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<SpecificationAttributeOption>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        Task<IEnumerable<SpecificationAttributeOption>> Filter(Guid id);
        void Update(IEnumerable<SpecificationAttributeOption> items);
        void Add(IEnumerable<SpecificationAttributeOption> items);
        void Remove(IEnumerable<SpecificationAttributeOption> t);
        Task<IEnumerable<SpecificationAttributeOption>> GetByParentId(Guid id);
    }
}
