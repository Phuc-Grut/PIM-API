using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductOriginRepository : IRepository<ProductOrigin>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<ProductOrigin>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductOrigin>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<ProductOrigin> productOrigins);
    }
}
