using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IManufacturerRepository : IRepository<Manufacturer>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<Manufacturer>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<Manufacturer>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<Manufacturer> manufacturers);
    }
}
