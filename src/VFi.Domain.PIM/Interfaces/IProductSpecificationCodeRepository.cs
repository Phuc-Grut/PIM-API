using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductSpecificationCodeRepository : IRepository<ProductSpecificationCode>
    {
        Task<IEnumerable<ProductSpecificationCode>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductSpecificationCode>> GetListListBox(Dictionary<string, object> filter);
        Task<IEnumerable<ProductSpecificationCode>> Filter(Guid Id);
        Task<IEnumerable<ProductSpecificationCode>> GetByParentId(Guid id);
        void Add(IEnumerable<ProductSpecificationCode> items);
        void Remove(IEnumerable<ProductSpecificationCode> t);
        void Update(IEnumerable<ProductSpecificationCode> t);
    }
}
