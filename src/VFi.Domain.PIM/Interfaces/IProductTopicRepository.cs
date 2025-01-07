using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductTopicRepository : IRepository<ProductTopic>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<ProductTopic>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductTopic>> GetListListBox(int? status, string? keyword, Guid? productTopicPageId);
        void Update(IEnumerable<ProductTopic> t); 
        Task<ProductTopic> GetBySlug(string slug);
        Task<ProductTopic> GetByCode(string code);
        Task<(IEnumerable<ProductTopic>, int)> Filter(string? keyword, Guid? productTopicPageId, IFopRequest request);
    }
}
