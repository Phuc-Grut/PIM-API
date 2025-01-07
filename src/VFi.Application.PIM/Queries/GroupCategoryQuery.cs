using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Filter;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class GroupCategoryQueryAll : IQuery<IEnumerable<GroupCategoryDto>>
    {
        public GroupCategoryQueryAll()
        {
        }
    }

    public class GroupCategoryQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public GroupCategoryQueryListBox(int? status, string? keyword)
        {
            Status = status;
            Keyword = keyword;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
    }
    public class GroupCategoryQueryCheckExist : IQuery<bool>
    {

        public GroupCategoryQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class GroupCategoryQueryById : IQuery<GroupCategoryDto>
    {
        public GroupCategoryQueryById()
        {
        }

        public GroupCategoryQueryById(Guid groupCategoryId)
        {
            GroupCategoryId = groupCategoryId;
        }

        public Guid GroupCategoryId { get; set; }
    }
    public class GroupCategoryQueryByCode : IQuery<GroupCategoryDto>
    {
        public GroupCategoryQueryByCode()
        {
        }

        public GroupCategoryQueryByCode(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
    public class GroupCategoryPagingQuery : FopQuery, IQuery<PagedResult<List<GroupCategoryDto>>>
    {
        public GroupCategoryPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string? Keyword { get; set; }
    }

    public class GroupCategoryQueryHandler : IQueryHandler<GroupCategoryQueryListBox, IEnumerable<ListBoxDto>>, 
                                             IQueryHandler<GroupCategoryQueryAll, IEnumerable<GroupCategoryDto>>, 
                                             IQueryHandler<GroupCategoryQueryCheckExist, bool>,
                                             IQueryHandler<GroupCategoryQueryById, GroupCategoryDto>,
                                             IQueryHandler<GroupCategoryQueryByCode, GroupCategoryDto>,
                                             IQueryHandler<GroupCategoryPagingQuery, PagedResult<List<GroupCategoryDto>>>
    {
        private readonly IGroupCategoryRepository _groupCategoryRepository;
        public GroupCategoryQueryHandler(IGroupCategoryRepository groupCategoryRespository)
        {
            _groupCategoryRepository = groupCategoryRespository;
        }
        public async Task<bool> Handle(GroupCategoryQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _groupCategoryRepository.CheckExistById(request.Id);
            
        }

        public async Task<GroupCategoryDto> Handle(GroupCategoryQueryById request, CancellationToken cancellationToken)
        {
            var groupCategory = await _groupCategoryRepository.GetById(request.GroupCategoryId);
            var result = new GroupCategoryDto()
            {
                Id = groupCategory.Id,
                Code = groupCategory.Code,
                Name = groupCategory.Name,
                Title = groupCategory.Title,
                Description= groupCategory.Description,
                Image = groupCategory.Image,
                Logo= groupCategory.Logo,
                Logo2= groupCategory.Logo2,
                Favicon= groupCategory.Favicon,
                Url= groupCategory.Url,
                Tags= groupCategory.Tags,
                Email= groupCategory.Email,
                Phone= groupCategory.Phone,
                Address= groupCategory.Address,
                Facebook= groupCategory.Facebook,
                Youtube= groupCategory.Youtube,
                Zalo= groupCategory.Zalo,
                DisplayOrder = groupCategory.DisplayOrder,
                Status = groupCategory.Status,
                CreatedBy = groupCategory.CreatedBy,
                CreatedDate = groupCategory.CreatedDate,
                UpdatedBy = groupCategory.UpdatedBy,
                UpdatedDate = groupCategory.UpdatedDate,
                CreatedByName = groupCategory.CreatedByName,
                UpdatedByName = groupCategory.UpdatedByName
            };
            return result;
        }
        public async Task<GroupCategoryDto> Handle(GroupCategoryQueryByCode request, CancellationToken cancellationToken)
        {
            var groupCategory = await _groupCategoryRepository.GetByCode(request.Code);
            var result = new GroupCategoryDto()
            {
                Id = groupCategory.Id,
                Code = groupCategory.Code,
                Name = groupCategory.Name,
                Title = groupCategory.Title,
                Description = groupCategory.Description,
                Image = groupCategory.Image,
                Logo = groupCategory.Logo,
                Logo2 = groupCategory.Logo2,
                Favicon = groupCategory.Favicon,
                Url = groupCategory.Url,
                Tags = groupCategory.Tags,
                Email = groupCategory.Email,
                Phone = groupCategory.Phone,
                Address = groupCategory.Address,
                Facebook = groupCategory.Facebook,
                Youtube = groupCategory.Youtube,
                Zalo = groupCategory.Zalo,
                DisplayOrder = groupCategory.DisplayOrder,
                Status = groupCategory.Status,
                CreatedBy = groupCategory.CreatedBy,
                CreatedDate = groupCategory.CreatedDate,
                UpdatedBy = groupCategory.UpdatedBy,
                UpdatedDate = groupCategory.UpdatedDate,
                CreatedByName = groupCategory.CreatedByName,
                UpdatedByName = groupCategory.UpdatedByName
            };
            return result;
        }
        public async Task<PagedResult<List<GroupCategoryDto>>> Handle(GroupCategoryPagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<GroupCategory>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<GroupCategoryDto>>();
            var (datas, count) = await _groupCategoryRepository.Filter(request.Keyword, fopRequest);
            var data = datas.Select(groupCategory => new GroupCategoryDto()
            {
                Id = groupCategory.Id,
                Code = groupCategory.Code,
                Name = groupCategory.Name,
                Title = groupCategory.Title,
                Description = groupCategory.Description,
                Image = groupCategory.Image,
                Favicon = groupCategory.Favicon,
                Url = groupCategory.Url,
                Tags = groupCategory.Tags,
                Email = groupCategory.Email,
                Phone = groupCategory.Phone,
                Address = groupCategory.Address,
                Facebook = groupCategory.Facebook,
                Youtube = groupCategory.Youtube,
                Zalo = groupCategory.Zalo,
                DisplayOrder = groupCategory.DisplayOrder,
                Status = groupCategory.Status,
                CreatedBy = groupCategory.CreatedBy,
                CreatedDate = groupCategory.CreatedDate,
                CreatedByName = groupCategory.CreatedByName,
                UpdatedBy = groupCategory.UpdatedBy,
                UpdatedDate = groupCategory.UpdatedDate,
                UpdatedByName = groupCategory.UpdatedByName,
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<GroupCategoryDto>> Handle(GroupCategoryQueryAll request, CancellationToken cancellationToken)
        {
            var groupCategorys = await _groupCategoryRepository.GetAll();
            var result = groupCategorys.Select(groupCategory => new GroupCategoryDto()
            {
                Id = groupCategory.Id,
                Code = groupCategory.Code,
                Name = groupCategory.Name,
                Title = groupCategory.Title,
                Description = groupCategory.Description,
                Image = groupCategory.Image,
                Logo = groupCategory.Logo,
                Logo2 = groupCategory.Logo2,
                Favicon = groupCategory.Favicon,
                Url = groupCategory.Url,
                Tags = groupCategory.Tags,
                Email = groupCategory.Email,
                Phone = groupCategory.Phone,
                Address = groupCategory.Address,
                Facebook = groupCategory.Facebook,
                Youtube = groupCategory.Youtube,
                Zalo = groupCategory.Zalo,
                DisplayOrder = groupCategory.DisplayOrder,
                Status = groupCategory.Status,
                CreatedBy = groupCategory.CreatedBy,
                CreatedDate = groupCategory.CreatedDate,
                UpdatedBy = groupCategory.UpdatedBy,
                UpdatedDate = groupCategory.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(GroupCategoryQueryListBox request, CancellationToken cancellationToken)
        {

            var groupCategorys = await _groupCategoryRepository.GetListListBox(request.Status, request.Keyword);
            var result = groupCategorys.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
