using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductSpecificationAttributeMappingRepository : IRepository<ProductSpecificationAttributeMapping>
    {
        Task<IEnumerable<ProductSpecificationAttributeMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductSpecificationAttributeMapping>> GetListListBox(Dictionary<string, object> filter);
        Task<IEnumerable<ProductSpecificationAttributeMapping>> Filter(Guid Id);
        void Add(IEnumerable<ProductSpecificationAttributeMapping> items);
        void Update(IEnumerable<ProductSpecificationAttributeMapping> t);
        void Remove(IEnumerable<ProductSpecificationAttributeMapping> t);
        Task<IEnumerable<ProductSpecificationAttributeMapping>> Filter(Dictionary<string, object> filter);
        Task<IEnumerable<ProductSpecificationAttributeMapping>> GetByParentId(Guid id);
    }
}
