using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IWarehouseRepository : IRepository<Warehouse>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<Warehouse>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword);
        Task<IEnumerable<Warehouse>> GetListListBox(string? keyword, int? status);
        void Update(IEnumerable<Warehouse> warehouses);
        Task<IEnumerable<Warehouse>> Filter(IEnumerable<ProductInventory> productInventories);
    }
}
