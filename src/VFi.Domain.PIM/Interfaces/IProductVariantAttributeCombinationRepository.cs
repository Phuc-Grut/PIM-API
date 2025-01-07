using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductVariantAttributeCombinationRepository : IRepository<ProductVariantAttributeCombination>
    {
        Task<IEnumerable<ProductVariantAttributeCombination>> Filter(Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(Dictionary<string, object> filter);
        Task<IEnumerable<ProductVariantAttributeCombination>> GetListListBox(Dictionary<string, object> filter);
    }
}
