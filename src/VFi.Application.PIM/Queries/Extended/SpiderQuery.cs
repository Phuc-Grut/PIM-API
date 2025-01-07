using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.NetDevPack.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Queries
{
    public class SpiderQuery : IQuery<string>
    {
        public SpiderQuery(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }

    public class SpiderQueryHandler : IQueryHandler<SpiderQuery, string>
    {
        private readonly ISpiderRepository _spiderRepository;

        public SpiderQueryHandler(ISpiderRepository spiderRepository)
        {
            _spiderRepository = spiderRepository;
        }

        public Task<string> Handle(SpiderQuery request, CancellationToken cancellationToken)
        {
            return _spiderRepository.Crawler(request.Url);
        }
    }
}
