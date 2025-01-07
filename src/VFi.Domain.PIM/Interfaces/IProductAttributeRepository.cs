using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductAttributeRepository : IRepository<ProductAttribute>
    {
        Task<IEnumerable<ProductAttribute>> Filter(string? keyword, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword);
        Task<IEnumerable<ProductAttribute>> GetListListBox(string? keyword, int? status);
        void Update(IEnumerable<ProductAttribute> attributes);
        Task<(IEnumerable<ProductAttribute>, int)> Filter(string? keyword, IFopRequest request);
    }
}
