using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ICurrencyRepository : IRepository<Currency>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<Currency>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<Currency>> GetListListBox(int? status, string? keyword);
    }
}
