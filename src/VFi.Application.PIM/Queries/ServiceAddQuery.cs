using Consul;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using MassTransit.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ServiceAddQueryAll : IQuery<IEnumerable<ServiceAddDto>>
    {
        public ServiceAddQueryAll()
        {
        }
    }

    public class ServiceAddQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ServiceAddQueryListBox(string? keyword, int? status)
        {
            Keyword = keyword;
            Status = status;
        }
        public string? Keyword { get; set; }
        public int? Status { get; set; }
    }
    public class ServiceAddQueryCheckExist : IQuery<bool>
    {

        public ServiceAddQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ServiceAddQueryById : IQuery<ServiceAddDto>
    {
        public ServiceAddQueryById()
        {
        }

        public ServiceAddQueryById(Guid serviceAddId)
        {
            ServiceAddId = serviceAddId;
        }

        public Guid ServiceAddId { get; set; }
    }
    public class ServiceAddPagingQuery : FopQuery, IQuery<PagedResult<List<ServiceAddDto>>>
    {
        public ServiceAddPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public string? Keyword { get; set; }
        public string? Filter { get; set; }
        public string? Order { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class ServiceAddQueryHandler : IQueryHandler<ServiceAddQueryListBox, IEnumerable<ListBoxDto>>,
                                             IQueryHandler<ServiceAddQueryAll, IEnumerable<ServiceAddDto>>,
                                             IQueryHandler<ServiceAddQueryCheckExist, bool>,
                                             IQueryHandler<ServiceAddQueryById, ServiceAddDto>,
                                             IQueryHandler<ServiceAddPagingQuery, PagedResult<List<ServiceAddDto>>>
    {
        private readonly IServiceAddRepository _serviceAddRepository;
        public ServiceAddQueryHandler(IServiceAddRepository serviceAddRespository)
        {
            _serviceAddRepository = serviceAddRespository;
        }
        public async Task<bool> Handle(ServiceAddQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _serviceAddRepository.CheckExistById(request.Id);
        }

        public async Task<ServiceAddDto> Handle(ServiceAddQueryById request, CancellationToken cancellationToken)
        {
            var serviceAdd = await _serviceAddRepository.GetById(request.ServiceAddId);
            var result = new ServiceAddDto()
            {
                Id = serviceAdd.Id,
                Code = serviceAdd.Code,
                Name = serviceAdd.Name,
                Description = serviceAdd.Description,
                CalculationMethod = serviceAdd.CalculationMethod,
                Price = serviceAdd.Price,
                PriceSyntax = serviceAdd.PriceSyntax,
                MinPrice = serviceAdd.MinPrice,
                MaxPrice = serviceAdd.MaxPrice,
                Status = serviceAdd.Status,
                Currency = serviceAdd.Currency,
                DisplayOrder = serviceAdd.DisplayOrder,
                CreatedBy = serviceAdd.CreatedBy,
                CreatedDate = serviceAdd.CreatedDate,
                CreatedByName = serviceAdd.CreatedByName,
                UpdatedBy = serviceAdd.UpdatedBy,
                UpdatedDate = serviceAdd.UpdatedDate,
                UpdatedByName = serviceAdd.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<ServiceAddDto>>> Handle(ServiceAddPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<ServiceAddDto>>();
            var fopRequest = FopExpressionBuilder<ServiceAdd>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (serviceAdds, count) = await _serviceAddRepository.Filter(request.Keyword, fopRequest);
            var data = serviceAdds.Select(serviceAdd => new ServiceAddDto()
            {
                Id = serviceAdd.Id,
                Code = serviceAdd.Code,
                Name = serviceAdd.Name,
                Description = serviceAdd.Description,
                CalculationMethod = serviceAdd.CalculationMethod,
                Price = serviceAdd.Price,
                PriceSyntax = serviceAdd.PriceSyntax,
                MinPrice = serviceAdd.MinPrice,
                MaxPrice = serviceAdd.MaxPrice,
                Status = serviceAdd.Status,
                Currency = serviceAdd.Currency,
                DisplayOrder = serviceAdd.DisplayOrder,
                CreatedBy = serviceAdd.CreatedBy,
                CreatedDate = serviceAdd.CreatedDate,
                CreatedByName = serviceAdd.CreatedByName,
                UpdatedBy = serviceAdd.UpdatedBy,
                UpdatedDate = serviceAdd.UpdatedDate,
                UpdatedByName = serviceAdd.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ServiceAddDto>> Handle(ServiceAddQueryAll request, CancellationToken cancellationToken)
        {
            var serviceAdds = await _serviceAddRepository.GetAll();
            var result = serviceAdds.Select(serviceAdd => new ServiceAddDto()
            {
                Id = serviceAdd.Id,
                Code = serviceAdd.Code,
                Name = serviceAdd.Name,
                Description = serviceAdd.Description,
                CalculationMethod = serviceAdd.CalculationMethod,
                Price = serviceAdd.Price,
                PriceSyntax = serviceAdd.PriceSyntax,
                MinPrice = serviceAdd.MinPrice,
                MaxPrice = serviceAdd.MaxPrice,
                Status = serviceAdd.Status,
                Currency = serviceAdd.Currency,
                DisplayOrder = serviceAdd.DisplayOrder,
                CreatedBy = serviceAdd.CreatedBy,
                CreatedDate = serviceAdd.CreatedDate,
                UpdatedBy = serviceAdd.UpdatedBy,
                UpdatedDate = serviceAdd.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ServiceAddQueryListBox request, CancellationToken cancellationToken)
        {

            var serviceAdds = await _serviceAddRepository.GetListListBox(request.Keyword, request.Status);
            var result = serviceAdds.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
