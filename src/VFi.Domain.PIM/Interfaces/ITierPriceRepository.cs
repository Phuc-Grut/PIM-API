using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ITierPriceRepository : IRepository<TierPrice>
    {
        Task<IEnumerable<TierPrice>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<TierPrice>> GetListListBox(Dictionary<string, object> filter);
        void Add(IEnumerable<TierPrice> items);
        void Remove(IEnumerable<TierPrice> t);
        void Update(IEnumerable<TierPrice> t);
        Task<IEnumerable<TierPrice>> GetByParentId(Guid id);
    }
}
