using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ISpecificationAttributeRepository : IRepository<SpecificationAttribute>
    {
        Task<IEnumerable<SpecificationAttribute>> Filter(string? keyword, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword);
        Task<IEnumerable<SpecificationAttribute>> GetListListBox(string? keyword, int? status);
        void Update(IEnumerable<SpecificationAttribute> attributes);
        Task<(IEnumerable<SpecificationAttribute>, int)> Filter(string? keyword, IFopRequest request);
    }
}
