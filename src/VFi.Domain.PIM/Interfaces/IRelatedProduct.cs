using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IRelatedProductRepository : IRepository<RelatedProduct>
    {
        Task<IEnumerable<RelatedProduct>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<RelatedProduct>> GetListListBox(Dictionary<string, object> filter);
        void Add(IEnumerable<RelatedProduct> items);
        Task<IEnumerable<RelatedProduct>> Filter(Guid Id);
        void Remove(IEnumerable<RelatedProduct> t);
    }
}
