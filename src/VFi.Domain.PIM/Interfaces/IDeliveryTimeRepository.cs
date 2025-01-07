using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IDeliveryTimeRepository : IRepository<DeliveryTime>
    {
        Task<IEnumerable<DeliveryTime>> Filter(string? keyword, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword);
        Task<IEnumerable<DeliveryTime>> GetListListBox(string? keyword);
        void Update(IEnumerable<DeliveryTime> deliveryTimes);
        Task<(IEnumerable<DeliveryTime>, int)> Filter(string? keyword, IFopRequest request);
    }
}
