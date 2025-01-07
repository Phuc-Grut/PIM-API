using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductGroupCategoryMappingRepository : IRepository<ProductGroupCategoryMapping>
    {
        Task<IEnumerable<ProductGroupCategoryMapping>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductGroupCategoryMapping>> GetListListBox(Dictionary<string, object> filter);
        void Add(IEnumerable<ProductGroupCategoryMapping> items);
        Task<IEnumerable<ProductGroupCategoryMapping>> Filter(Guid Id);

        void Remove(IEnumerable<ProductGroupCategoryMapping> t);
    }
}
