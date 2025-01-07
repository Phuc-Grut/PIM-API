using VFi.Domain.PIM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface ISyntaxCodeRepository
    {
        Task<string> GetCode(string syntax,int use);
        Task<int> UseCode(string syntax, string code);
        Task<IEnumerable<Proxy>> GetList(string? group);
    }
}
