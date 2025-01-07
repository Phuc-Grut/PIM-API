using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductTopicPageRepository : IRepository<ProductTopicPage>
    {
        Task<IEnumerable<ProductTopicPage>> GetAll(int? status);
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<ProductTopicPage>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductTopicPage>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<ProductTopicPage> t); 
        Task<ProductTopicPage> GetByCode(string code);
        Task<IEnumerable<ProductTopicPageMap>> GetProductTopicPageMapByCode(string code);
        Task<ProductTopicPage> GetBySlug(string slug);
        Task<(IEnumerable<ProductTopicPage>, int)> Filter(string? keyword, IFopRequest request);
    }
}
