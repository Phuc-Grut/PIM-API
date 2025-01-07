using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IProductBrandRepository : IRepository<ProductBrand>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<(IEnumerable<ProductBrand>, int)> Filter(string? keyword, IFopRequest request);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductBrand>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<ProductBrand> productBrands);
    }
}
