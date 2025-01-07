using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Queries
{
    public class ServiceAddPriceSyntaxComboboxQuery: IQuery<IEnumerable<ServiceAddPriceSyntaxCombobox>>
    {
    }

    public class ServiceAddPriceSyntaxQueryHandler : IQueryHandler<ServiceAddPriceSyntaxComboboxQuery, IEnumerable<ServiceAddPriceSyntaxCombobox>>
    {
        private readonly IServiceAddPriceSyntaxRepository _serviceAddPriceSyntaxRepository;
        public ServiceAddPriceSyntaxQueryHandler(IServiceAddPriceSyntaxRepository serviceAddPriceSyntaxRepository) 
        {
            _serviceAddPriceSyntaxRepository = serviceAddPriceSyntaxRepository;
        }

        public async Task<IEnumerable<ServiceAddPriceSyntaxCombobox>> Handle(ServiceAddPriceSyntaxComboboxQuery request, CancellationToken cancellationToken)
        {
            var data = await _serviceAddPriceSyntaxRepository.GetAll();
            var result = data.Select(x => new ServiceAddPriceSyntaxCombobox
            {
                Value = x.Id,
                Key = x.Code,
                Label = x.Name,
                Description = x.Description,
                Type = "parameters",
            });

            return result;
        }
    }
}
