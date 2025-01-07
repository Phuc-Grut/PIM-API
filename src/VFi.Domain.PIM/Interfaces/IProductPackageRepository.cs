using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductPackageRepository : IRepository<ProductPackage>
    {
        Task<IEnumerable<ProductPackage>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductPackage>> GetListListBox(Dictionary<string, object> filter);
        Task<IEnumerable<ProductPackage>> GetByParentId(Guid id);
        Task<List<ProductPackage>> GetByProducts(string ids);
        void Add(IEnumerable<ProductPackage> items);
        void Remove(IEnumerable<ProductPackage> t);
        void Update(IEnumerable<ProductPackage> t);
    }
}
