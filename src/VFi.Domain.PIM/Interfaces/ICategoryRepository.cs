using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;
using System.Linq.Expressions;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ICategoryRepository : IRepository<Category>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<Category>> Filter(string? keyword, Dictionary<string, object> filter);
        Task<(IEnumerable<Category>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
        Task<IEnumerable<Category>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<Category>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        Task<IEnumerable<Category>> GetCombobox(Dictionary<string, object> filter, string? keyword);
        void Update(IEnumerable<Category> stores);
        Task<IEnumerable<Category>> GetByIds(IEnumerable<ProductCategoryMapping> productCategoryMapping);
        Task<IEnumerable<Category>> GetBreadcrumb(string group, string category);
        Task<Category> GetByCode(string code, string groupcode);
        Task<(IEnumerable<Category>, int)> Filter(string? keyword, IFopRequest request);
        void UpdateField(Category category, params Expression<Func<Category, object>>[] properties);
    }
}
