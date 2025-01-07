using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IStoreRepository : IRepository<Store>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<Store>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword);
        Task<IEnumerable<Store>> GetListListBox(string? keyword);
        void Update(IEnumerable<Store> stores);
        Task<IEnumerable<Store>> GetById(IEnumerable<ProductStoreMapping> productStoreMapping);
    }
}
