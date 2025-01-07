using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IUnitRepository : IRepository<Unit>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<Unit>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<Unit>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        void Update(IEnumerable<Unit> units);
    }
}
