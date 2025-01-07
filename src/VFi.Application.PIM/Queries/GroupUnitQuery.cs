using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class GroupUnitQueryAll : IQuery<IEnumerable<GroupUnitDto>>
    {
        public GroupUnitQueryAll()
        {
        }
    }

    public class GroupUnitQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public GroupUnitQueryListBox(int? status, string? keyword)
        {
            Status = status;
            Keyword = keyword;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
    }
    public class GroupUnitQueryCheckExist : IQuery<bool>
    {

        public GroupUnitQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class GroupUnitQueryById : IQuery<GroupUnitDto>
    {
        public GroupUnitQueryById()
        {
        }

        public GroupUnitQueryById(Guid groupUnitId)
        {
            GroupUnitId = groupUnitId;
        }

        public Guid GroupUnitId { get; set; }
    }
    public class GroupUnitPagingQuery : FopQuery, IQuery<PagedResult<List<GroupUnitDto>>>
    {
        public GroupUnitPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
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

    public class GroupUnitQueryHandler : IQueryHandler<GroupUnitQueryListBox, IEnumerable<ListBoxDto>>,
                                             IQueryHandler<GroupUnitQueryAll, IEnumerable<GroupUnitDto>>,
                                             IQueryHandler<GroupUnitQueryCheckExist, bool>,
                                             IQueryHandler<GroupUnitQueryById, GroupUnitDto>,
                                             IQueryHandler<GroupUnitPagingQuery, PagedResult<List<GroupUnitDto>>>
    {
        private readonly IGroupUnitRepository _groupUnitRepository;
        public GroupUnitQueryHandler(IGroupUnitRepository groupUnitRespository)
        {
            _groupUnitRepository = groupUnitRespository;
        }
        public async Task<bool> Handle(GroupUnitQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _groupUnitRepository.CheckExistById(request.Id);
        }

        public async Task<GroupUnitDto> Handle(GroupUnitQueryById request, CancellationToken cancellationToken)
        {
            var groupUnit = await _groupUnitRepository.GetById(request.GroupUnitId);
            var result = new GroupUnitDto()
            {
                Id = groupUnit.Id,
                Code = groupUnit.Code,
                Name = groupUnit.Name,
                Description = groupUnit.Description,
                DisplayOrder = groupUnit.DisplayOrder,
                Status = groupUnit.Status,
                CreatedBy = groupUnit.CreatedBy,
                CreatedDate = groupUnit.CreatedDate,
                CreatedByName = groupUnit.CreatedByName,
                UpdatedBy = groupUnit.UpdatedBy,
                UpdatedDate = groupUnit.UpdatedDate,
                UpdatedByName = groupUnit.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<GroupUnitDto>>> Handle(GroupUnitPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<GroupUnitDto>>();
            var fopRequest = FopExpressionBuilder<Domain.PIM.Models.GroupUnit>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (groupUnits, count) = await _groupUnitRepository.Filter(request.Keyword, fopRequest);
            var data = groupUnits.Select(groupUnit => new GroupUnitDto()
            {
                Id = groupUnit.Id,
                Code = groupUnit.Code,
                Name = groupUnit.Name,
                Description = groupUnit.Description,
                DisplayOrder = groupUnit.DisplayOrder,
                Status = groupUnit.Status,
                CreatedBy = groupUnit.CreatedBy,
                CreatedDate = groupUnit.CreatedDate,
                CreatedByName = groupUnit.CreatedByName,
                UpdatedBy = groupUnit.UpdatedBy,
                UpdatedDate = groupUnit.UpdatedDate,
                UpdatedByName = groupUnit.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<GroupUnitDto>> Handle(GroupUnitQueryAll request, CancellationToken cancellationToken)
        {
            var groupUnits = await _groupUnitRepository.GetAll();
            var result = groupUnits.Select(groupUnit => new GroupUnitDto()
            {
                Id = groupUnit.Id,
                Code = groupUnit.Code,
                Name = groupUnit.Name,
                Description = groupUnit.Description,
                DisplayOrder = groupUnit.DisplayOrder,
                Status = groupUnit.Status,
                CreatedBy = groupUnit.CreatedBy,
                CreatedDate = groupUnit.CreatedDate,
                UpdatedBy = groupUnit.UpdatedBy,
                UpdatedDate = groupUnit.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(GroupUnitQueryListBox request, CancellationToken cancellationToken)
        {

            var groupUnits = await _groupUnitRepository.GetListListBox(request.Status, request.Keyword);
            var result = groupUnits.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
