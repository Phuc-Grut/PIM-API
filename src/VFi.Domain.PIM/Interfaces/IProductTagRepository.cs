using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductTagRepository : IRepository<ProductTag>
    {
        Task<IEnumerable<ProductTag>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductTag>> GetListListBox(int? status, int? type, string? keyword);
        Task<IEnumerable<ProductTag>> GetByIds(IEnumerable<ProductProductTagMapping> productProductTagMapping);


        Task<IEnumerable<Guid>> Filter(List<string> tags, Guid createdBy);
        Task<(IEnumerable<ProductTag>, int)> Filter(string? keyword, IFopRequest request);
    }
}
