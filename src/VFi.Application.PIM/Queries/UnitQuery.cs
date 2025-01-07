using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using MediatR;
using System.Data;

namespace VFi.Application.PIM.Queries
{

    public class UnitQueryAll : IQuery<IEnumerable<UnitDto>>
    {
        public UnitQueryAll()
        {
        }
    }

    public class UnitQueryListBox : IQuery<IEnumerable<ListBoxUnitDto>>
    {
        public UnitQueryListBox(int? status, string? groupId, string? keyword, bool nullable)
        {
            Keyword = keyword;
            Filter = new Dictionary<string, object>();
            if (status != null)
            {
                Filter.Add("status", status);
            }
            if (!String.IsNullOrEmpty(groupId))
            {
                Filter.Add("groupId", groupId);
            }
            else
            {
                if (nullable) Filter.Add("groupId", null);
            }
        }
        public string? Keyword { get; set; }
        public Dictionary<string, object> Filter { get; set; }
    }
    public class UnitQueryCheckExist : IQuery<bool>
    {

        public UnitQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class UnitQueryById : IQuery<UnitDto>
    {
        public UnitQueryById()
        {
        }

        public UnitQueryById(Guid unitId)
        {
            UnitId = unitId;
        }

        public Guid UnitId { get; set; }
    }
    public class UnitPagingQuery : FopQuery, IQuery<PagedResult<List<UnitDto>>>
    {
        public UnitPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
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

    public class UnitQueryHandler : IQueryHandler<UnitQueryListBox, IEnumerable<ListBoxUnitDto>>,
                                             IQueryHandler<UnitQueryAll, IEnumerable<UnitDto>>,
                                             IQueryHandler<UnitQueryCheckExist, bool>,
                                             IQueryHandler<UnitQueryById, UnitDto>,
                                             IQueryHandler<UnitPagingQuery, PagedResult<List<UnitDto>>>
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IGroupUnitRepository _groupUnitRepository;
        public UnitQueryHandler(IUnitRepository unitRespository, IGroupUnitRepository groupUnitRepository)
        {
            _unitRepository = unitRespository;
            _groupUnitRepository = groupUnitRepository;
        }
        public async Task<bool> Handle(UnitQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _unitRepository.CheckExistById(request.Id);
        }

        public async Task<UnitDto> Handle(UnitQueryById request, CancellationToken cancellationToken)
        {
            var unit = await _unitRepository.GetById(request.UnitId);
            var result = new UnitDto()
            {
                Id = unit.Id,
                Code = unit.Code,
                Name = unit.Name,
                Description = unit.Description,
                GroupUnitId = unit.GroupUnitId,
                GroupUnitName = unit.GroupUnitId != null ? unit.GroupUnit.Name : null,
                DisplayOrder = unit.DisplayOrder,
                DisplayLocale = unit.DisplayLocale,
                IsDefault = unit.IsDefault,
                NamePlural = unit.NamePlural,
                Rate = unit.Rate,
                Status = unit.Status,
                CreatedBy = unit.CreatedBy,
                CreatedDate = unit.CreatedDate,
                CreatedByName = unit.CreatedByName,
                UpdatedBy = unit.UpdatedBy,
                UpdatedDate = unit.UpdatedDate,
                UpdatedByName = unit.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<UnitDto>>> Handle(UnitPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<UnitDto>>();
            var fopRequest = FopExpressionBuilder<Domain.PIM.Models.Unit>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (datas, count) = await _unitRepository.Filter(request.Keyword, fopRequest);
            var _group = await _groupUnitRepository.GetAll();
            var data = datas.Select(unit =>
            {
                var groupUnit = _group.Where(x => x.Id == unit.GroupUnitId).FirstOrDefault();
                return new UnitDto()
                {
                    Id = unit.Id,
                    Code = unit.Code,
                    Name = unit.Name,
                    Description = unit.Description,
                    GroupUnitId = unit.GroupUnitId,
                    GroupUnitName = unit.GroupUnitId != null ? groupUnit.Name : null,
                    GroupUnitCode = unit.GroupUnitId != null ? groupUnit.Code : null,
                    DisplayOrder = unit.DisplayOrder,
                    DisplayLocale = unit.DisplayLocale,
                    IsDefault = unit.IsDefault,
                    NamePlural = unit.NamePlural,
                    Rate = unit.Rate,
                    Status = unit.Status,
                    CreatedBy = unit.CreatedBy,
                    CreatedDate = unit.CreatedDate,
                    CreatedByName = unit.CreatedByName,
                    UpdatedBy = unit.UpdatedBy,
                    UpdatedDate = unit.UpdatedDate,
                    UpdatedByName = unit.UpdatedByName
                };
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<UnitDto>> Handle(UnitQueryAll request, CancellationToken cancellationToken)
        {
            var units = await _unitRepository.GetAll();
            var result = units.Select(unit => new UnitDto()
            {
                Id = unit.Id,
                Code = unit.Code,
                Name = unit.Name,
                Description = unit.Description,
                GroupUnitId = unit.GroupUnitId,
                GroupUnitName = unit.GroupUnitId != null ? unit.GroupUnit.Name : null,
                DisplayOrder = unit.DisplayOrder,
                DisplayLocale = unit.DisplayLocale,
                IsDefault = unit.IsDefault,
                NamePlural = unit.NamePlural,
                Rate = unit.Rate,
                Status = unit.Status,
                CreatedBy = unit.CreatedBy,
                CreatedDate = unit.CreatedDate,
                UpdatedBy = unit.UpdatedBy,
                UpdatedDate = unit.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxUnitDto>> Handle(UnitQueryListBox request, CancellationToken cancellationToken)
        {

            var units = await _unitRepository.GetListListBox(request.Filter, request.Keyword);
            var data = units.Select(data => new ListBoxUnitDto()
            {
                Value = data.Id,
                Label = data.Name,
                Key = data.Code,
                GroupUnitId = data.GroupUnitId
            });
            var result = data;
            return result;
        }
    }
}
