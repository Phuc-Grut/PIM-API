using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Domain.PIM.QueryModels;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Context;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Repository
{
    public partial class SyntaxCodeRepository : ISyntaxCodeRepository
    {
      
        private readonly MasterApiContext _apiContext;
        private readonly string PATH_GET_CODE = "/api/codesyntax/get-code";
        private readonly string PATH_USE_CODE = "/api/codesyntax/usecode"; 
        private readonly string PATH_GET_PROXY = "/api/proxy/get-listview";
        private readonly IContextUser _context;
        public SyntaxCodeRepository(MasterApiContext apiContext, IContextUser context) {
            _apiContext = apiContext;
            _context= context;
        }
     

        public Task<string> GetCode(string syntax, int use)
        {
            _apiContext.Token = _context.GetToken();
            var t = _apiContext.Client.Request(PATH_GET_CODE)
                                 .SetQueryParam("$syntaxCode", syntax)
                                 .SetQueryParam("$status", use)
                                 .GetJsonAsync().Result;
            return Task.FromResult(t.code);
        }

        public Task<int> UseCode(string syntax, string code)
        {
            _apiContext.Token = _context.GetToken();
            var t = _apiContext.Client.Request(PATH_USE_CODE)
                             .SetQueryParam("syntax", syntax)
                             .SetQueryParam("code", code)
                             .PostAsync().Result;
            return Task.FromResult(t.StatusCode);
        }
        public Task<IEnumerable<Proxy>> GetList(string? group)
        {
            _apiContext.Token = _context.GetToken();
            var t = _apiContext.Client.Request(PATH_GET_PROXY)
                             .SetQueryParam("group", group)
                             .SetQueryParam("status", 1)
                             .GetJsonAsync<IEnumerable<Proxy>>().Result;
            return Task.FromResult(t);
        }
    }
}
