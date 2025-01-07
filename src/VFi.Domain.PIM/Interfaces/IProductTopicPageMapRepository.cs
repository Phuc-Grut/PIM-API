using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductTopicPageMapRepository : IRepository<ProductTopicPageMap>
    {
        Task<IEnumerable<ProductTopic>> GetProductTopic(string page);
        Task<IEnumerable<ProductTopic>> GetProductTopicBySlugPage(string slug);
        Task<IEnumerable<Guid>> GetListProductTopicIdByPage(string page);
        Task<IEnumerable<Guid>> GetListProductTopicIdBySlugPage(string slug);
        Task<IEnumerable<ProductTopic>> GetProductTopicByPageId(Guid pageId);
        Task<ProductTopicPageMap> GetProductTopicMapByProductTopicId(Guid productTopicId);
        void Add(IEnumerable<ProductTopicPageMap> data);
        void Remove(IEnumerable<ProductTopicPageMap> data);
        Task<IEnumerable<ProductTopicPageMap>> Filter(Guid productTopicId, Guid productTopicPageId);
        Task<IEnumerable<ProductTopicPageMap>> Filter(Guid productTopicId);
    }
}
