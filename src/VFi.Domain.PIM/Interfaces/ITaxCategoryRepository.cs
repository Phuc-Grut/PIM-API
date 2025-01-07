using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ITaxCategoryRepository : IRepository<TaxCategory>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<TaxCategory>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<TaxCategory>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<TaxCategory> taxCategorys);
        Task<(IEnumerable<TaxCategory>, int)> Filter(string? keyword, IFopRequest request);
    }
}
