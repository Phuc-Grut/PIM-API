using VFi.Domain.PIM.Models;
using VFi.Domain.PIM.QueryModels;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries; 
namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductRepository : IRepository<Product>

    { 
        Task<Product> GetByCode(string code);
        Task<string> GetByCategoryRootId(Guid id);
        Task<(IEnumerable<Product>, int)> Filter(string? keyword,IFopRequest request);
        Task<IEnumerable<Product>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        Task<IEnumerable<Product>> Filter(IEnumerable<RelatedProduct> request);
        Task<IEnumerable<Product>> Filter(List<string> request);
        Task<Product> CheckExistAttrJson(string? attrJson, Guid parentId);
        Task<IEnumerable<Product>> Filter(Dictionary<string, object> filter);
        Task<int> AddProductSimple(ProductSimple item);
    }
}
