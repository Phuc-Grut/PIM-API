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
    public partial interface IProductTypeRepository : IRepository<ProductType>
    {
        Task<Boolean> CheckExist(string? code, Guid? id);
        Task<IEnumerable<ProductType>> Filter(string? keyword, Dictionary<string, object> filter, int pagesize, int pageindex);
        Task<int> FilterCount(string? keyword, Dictionary<string, object> filter);
        Task<IEnumerable<ProductType>> GetListListBox(int? status, string? keyword);
        void Update(IEnumerable<ProductType> productOrigins);

        Task<(IEnumerable<ProductType>, int)> Filter(string? keyword, IFopRequest request);
    }
}
