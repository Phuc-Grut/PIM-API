using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductTopicDetailRepository : IRepository<ProductTopicDetail>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<ProductTopicDetail>> GetAllByStatus(int status);
        Task<IEnumerable<ProductTopicDetail>> Filter(string? keyword, Dictionary<string, object> filter, Guid? productTopicId, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter, Guid? productTopicId);
        Task<IEnumerable<ProductTopicDetail>> GetListListBox(string? keyword);
        void Update(IEnumerable<ProductTopicDetail> warehouses);

        Task<IEnumerable<ProductTopicDetail>> FilterByPage(string? keyword, Dictionary<string, object> filter, List<Guid> productTopicListId, int pagesize, int pageindex);
        Task<int> FilterByPageCount(string? keyword, Dictionary<string, object> filter, List<Guid> productTopicListId);
    }
}
