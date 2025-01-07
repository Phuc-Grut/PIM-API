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
    public interface IProductTopicQueryRepository : IRepository<ProductTopicQuery>
    {
        Task<IEnumerable<ProductTopicQuery>> Filter(int? status, string? keyword, Guid? productTopicId);
        Task<(IEnumerable<ProductTopicQuery>, int)> Filter(string? keyword, IFopRequest request);
        void Update(IEnumerable<ProductTopicQuery> t);
    }
}
