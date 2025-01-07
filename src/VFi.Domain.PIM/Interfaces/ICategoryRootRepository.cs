using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ICategoryRootRepository : IRepository<CategoryRoot>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<CategoryRoot>> Filter(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<CategoryRoot>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<CategoryRoot>> GetListListBox(Dictionary<string, object> filter, string? keyword);
        void Update(IEnumerable<CategoryRoot> stores);
        Task<IEnumerable<CategoryRoot>> GetByIds(IEnumerable<Product> productMapping);
        Task<(IEnumerable<CategoryRoot>, int)> Filter(string? keyword, Dictionary<string, object> filter, IFopRequest request);
    }
}
