using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Context;
using Flurl.Http;
using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public partial class MasterRepository : IMasterRepository
    {
        private readonly MasterApiContext _apiContext;
        private readonly string PATH_GET_PROXY = "/api/proxy/get-listview";
        public MasterRepository(MasterApiContext apiContext)
        {
            _apiContext = apiContext;
        }
      //  private readonly IContextUser _context;

        public Task<IEnumerable<Proxy>> GetList(string? group)
        {
            //_apiContext.Token = _context.GetToken();
            var t = _apiContext.Client.Request(PATH_GET_PROXY)
                             .SetQueryParam("group", group)
                             .SetQueryParam("status", 1)
                             .GetJsonAsync<IEnumerable<Proxy>>().Result;
            return Task.FromResult(t);
        }
    }
}
