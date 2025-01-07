using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IGroupUnitRepository : IRepository<GroupUnit>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<GroupUnit>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<GroupUnit>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<GroupUnit> groupUnits);
    }
}
