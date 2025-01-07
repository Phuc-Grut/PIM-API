using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductVariantAttributeValueRepository : IRepository<ProductVariantAttributeValue>
    {
        Task<IEnumerable<ProductVariantAttributeValue>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductVariantAttributeValue>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        Task<IEnumerable<ProductVariantAttributeValue>> GetByParentId(Guid id);
        Task<IEnumerable<ProductVariantAttributeValue>> GetByParentId(IEnumerable<ProductProductAttributeMapping> list);
        void Add(IEnumerable<ProductVariantAttributeValue> items);
        void Remove(IEnumerable<ProductVariantAttributeValue> t);
        void Update(IEnumerable<ProductVariantAttributeValue> t);
    }
}
