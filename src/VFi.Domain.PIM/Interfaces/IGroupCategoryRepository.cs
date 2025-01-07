using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IGroupCategoryRepository : IRepository<GroupCategory>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<GroupCategory>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<GroupCategory>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<GroupCategory> t); 
        Task<GroupCategory> GetByCode(string code);
        Task<(IEnumerable<GroupCategory>, int)> Filter(string? keyword, IFopRequest request);
    }
}
