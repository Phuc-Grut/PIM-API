using Consul;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class DeliveryTimeQueryAll : IQuery<IEnumerable<DeliveryTimeDto>>
    {
        public DeliveryTimeQueryAll()
        {
        }
    }

    public class DeliveryTimeQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public DeliveryTimeQueryListBox(string? keyword)
        {
            Keyword = keyword;
        }
        public string? Keyword { get; set; }
    }
    public class DeliveryTimeQueryById : IQuery<DeliveryTimeDto>
    {
        public DeliveryTimeQueryById()
        {
        }

        public DeliveryTimeQueryById(Guid deliveryTimeId)
        {
            DeliveryTimeId = deliveryTimeId;
        }

        public Guid DeliveryTimeId { get; set; }
    }
    public class DeliveryTimeQueryCheckExist : IQuery<bool>
    {
        public DeliveryTimeQueryCheckExist(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
    public class DeliveryTimePagingQuery : FopQuery, IQuery<PagedResult<List<DeliveryTimeDto>>>
    {
        public DeliveryTimePagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public string? Keyword { get; set; }
    }

    public class DeliveryTimeQueryHandler : IQueryHandler<DeliveryTimeQueryListBox, IEnumerable<ListBoxDto>>, 
                                            IQueryHandler<DeliveryTimeQueryAll, IEnumerable<DeliveryTimeDto>>, 
                                            IQueryHandler<DeliveryTimeQueryById, DeliveryTimeDto>, 
                                            IQueryHandler<DeliveryTimeQueryCheckExist, bool>, 
                                            IQueryHandler<DeliveryTimePagingQuery, PagedResult<List<DeliveryTimeDto>>>
    {
        private readonly IDeliveryTimeRepository _deliveryTimeRepository;
        public DeliveryTimeQueryHandler(IDeliveryTimeRepository deliveryTimeRespository)
        {
            _deliveryTimeRepository = deliveryTimeRespository;
        }
        public async Task<bool> Handle(DeliveryTimeQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _deliveryTimeRepository.CheckExistById(request.Id);
        }
        public async Task<DeliveryTimeDto> Handle(DeliveryTimeQueryById request, CancellationToken cancellationToken)
        {
            var deliveryTime = await _deliveryTimeRepository.GetById(request.DeliveryTimeId);
            var result = new DeliveryTimeDto()
            {
                Id = deliveryTime.Id,
                Name = deliveryTime.Name,
                IsDefault= deliveryTime.IsDefault,
                MaxDays= deliveryTime.MaxDays,
                MinDays= deliveryTime.MinDays,
                DisplayOrder = deliveryTime.DisplayOrder,
                Status = deliveryTime.Status,
                CreatedBy = deliveryTime.CreatedBy,
                CreatedDate = deliveryTime.CreatedDate,
                CreatedByName = deliveryTime.CreatedByName,
                UpdatedBy = deliveryTime.UpdatedBy,
                UpdatedDate = deliveryTime.UpdatedDate,
                UpdatedByName = deliveryTime.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<DeliveryTimeDto>>> Handle(DeliveryTimePagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<DeliveryTime>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<DeliveryTimeDto>>();
            var (datas, count) = await _deliveryTimeRepository.Filter(request.Keyword, fopRequest);
            var data = datas.Select(deliveryTime => new DeliveryTimeDto()
            {
                Id = deliveryTime.Id,
                Name = deliveryTime.Name,
                IsDefault = deliveryTime.IsDefault,
                MaxDays = deliveryTime.MaxDays,
                MinDays = deliveryTime.MinDays,
                DisplayOrder = deliveryTime.DisplayOrder,
                Status = deliveryTime.Status,
                CreatedBy = deliveryTime.CreatedBy,
                CreatedDate = deliveryTime.CreatedDate,
                CreatedByName = deliveryTime.CreatedByName,
                UpdatedBy = deliveryTime.UpdatedBy,
                UpdatedDate = deliveryTime.UpdatedDate,
                UpdatedByName = deliveryTime.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<DeliveryTimeDto>> Handle(DeliveryTimeQueryAll request, CancellationToken cancellationToken)
        {
            var deliveryTimes = await _deliveryTimeRepository.GetAll();
            var result = deliveryTimes.Select(deliveryTime => new DeliveryTimeDto()
            {
                Id = deliveryTime.Id,
                Name = deliveryTime.Name,
                IsDefault = deliveryTime.IsDefault,
                MaxDays = deliveryTime.MaxDays,
                MinDays = deliveryTime.MinDays,
                DisplayOrder = deliveryTime.DisplayOrder,
                Status = deliveryTime.Status,
                CreatedBy = deliveryTime.CreatedBy,
                CreatedDate = deliveryTime.CreatedDate,
                UpdatedBy = deliveryTime.UpdatedBy,
                UpdatedDate = deliveryTime.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(DeliveryTimeQueryListBox request, CancellationToken cancellationToken)
        {

            var deliveryTimes = await _deliveryTimeRepository.GetListListBox(request.Keyword);
            var result = deliveryTimes.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
