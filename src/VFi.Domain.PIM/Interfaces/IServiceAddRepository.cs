using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IServiceAddRepository : IRepository<ServiceAdd>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<ServiceAdd>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword, int? status);
        Task<IEnumerable<ServiceAdd>> GetListListBox(string? keyword, int? status);
        void Update(IEnumerable<ServiceAdd> serviceAdds);
    }
}
