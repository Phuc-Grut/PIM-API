using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductCategoryMappingRepository : IRepository<ProductCategoryMapping>
    {
        Task<IEnumerable<ProductCategoryMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductCategoryMapping>> GetListListBox(Dictionary<string, object> filter);
        void Add(IEnumerable<ProductCategoryMapping> items);
        Task<IEnumerable<ProductCategoryMapping>> Filter(Guid Id);
        void Remove(IEnumerable<ProductCategoryMapping> t);
    }
}
