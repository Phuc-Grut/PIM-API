using Consul;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;

namespace VFi.Application.PIM.Queries
{
    public class CategoryQueryByLevel : IQuery<IEnumerable<CategoryListViewDto>>
    {

        public CategoryQueryByLevel(Int32 status, string groupCode, Int32 level, Int32 levelCount)
        {
            Level = level;
            LevelCount = levelCount;
            GroupCode = groupCode;
            Status = status;
        }
        public CategoryQueryByLevel(Int32 status, string groupCode, String? parent, Int32? level, Int32? levelCount)
        {
            Level = level;
            LevelCount = levelCount;
            GroupCode = groupCode;
            Status = status;
            Parent = parent;
        }
        public CategoryQueryByLevel(Int32? status, string groupCode, String? parent)
        {

            GroupCode = groupCode;
            Status = status;
            Parent = parent;
        }
        public String GroupCode { get; set; }
        public Int32? Level { get; set; }
        public Int32? LevelCount { get; set; }
        public Int32? Status { get; set; }
        public String? Parent { get; set; }
    }
    public class CategoryQueryBreadcrumb : IQuery<IEnumerable<CategoryListViewDto>>
    {
        public CategoryQueryBreadcrumb(string group, string category)
        {
            Group = group;
            Category = category;
        }

        public string Group { get; set; }
        public string Category { get; set; }
    }
    public class CategoryQueryAll : IQuery<IEnumerable<CategoryDto>>
    {
        public CategoryQueryAll()
        {
        }
    }

    public class CategoryQueryListBox : IQuery<IEnumerable<CategoryListBoxDto>>
    {
        public CategoryQueryListBox(CategoryQueryParams @params, string? keyword)
        {
            Keyword = keyword;
            Filter = new Dictionary<string, object>();
            if (@params.Status != null)
            {
                Filter.Add("status", @params.Status);
            }
            if (!String.IsNullOrEmpty(@params.GroupCategoryId))
            {
                Filter.Add("groupId", @params.GroupCategoryId);
            }
            if (!String.IsNullOrEmpty(@params.ParentCategoryId))
            {
                Filter.Add("parentId", @params.ParentCategoryId);
            }
        }
        public string? Keyword { get; set; }
        public Dictionary<string, object> Filter { get; set; }
    }
    public class CategoryQueryCombobox : IQuery<IEnumerable<CategoryComboboxDto>>
    {
        public CategoryQueryCombobox(CategoryQueryParams @params, string? keyword)
        {
            Keyword = keyword;
            Filter = new Dictionary<string, object>();
            if (@params.Status != null)
            {
                Filter.Add("status", @params.Status);
            }
            if (!String.IsNullOrEmpty(@params.GroupCategoryId))
            {
                Filter.Add("groupId", @params.GroupCategoryId);
            }
            if (!String.IsNullOrEmpty(@params.ParentCategoryId))
            {
                Filter.Add("parentId", @params.ParentCategoryId);
            }
            else
            {
                Filter.Add("parentId", "null");
            }
        }
        public string? Keyword { get; set; }
        public Dictionary<string, object> Filter { get; set; }
    }
    public class CategoryQueryCheckExist : IQuery<bool>
    {

        public CategoryQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class CategoryQueryById : IQuery<CategoryDto>
    {
        public CategoryQueryById()
        {
        }

        public CategoryQueryById(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public Guid CategoryId { get; set; }
    }
    public class CategoryQueryByCode : IQuery<CategoryDto>
    {
        public CategoryQueryByCode()
        {
        }

        public CategoryQueryByCode(string groupcode, string code)
        {
            Code = code;
            Group = groupcode;
        }

        public string Code { get; set; }
        public string Group { get; set; }
    }
    public class CategoryQueryAllParent : IQuery<IEnumerable<CategoryParentDto>>
    {
        public CategoryQueryAllParent()
        {
        }

        public CategoryQueryAllParent(Guid categoryId)
        {
            CategoryId = categoryId;
        }

        public Guid CategoryId { get; set; }
    }
    public class CategoryPagingQuery : FopQuery, IQuery<PagedResult<List<CategoryDto>>>
    {
        public CategoryPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize, int? status, string? groupCategoryId, string? parentCategoryId)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Status = status;
            GroupCategoryId = groupCategoryId;
            ParentCategoryId = parentCategoryId;
        }

        public int? Status { get; set; }
        public string? Keyword { get; set; }
        public string? GroupCategoryId { get; set; }
        public string? ParentCategoryId { get; set; }
    }

    public class SearchCategoryQuery : ListQuery, IQuery<PagingResponse<CategoryDto>>
    {
        public SearchCategoryQuery(int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {

        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
        public string? Group { get; set; }
    }
    public class CategoryQueryHandler : IQueryHandler<CategoryQueryListBox, IEnumerable<CategoryListBoxDto>>, IQueryHandler<CategoryQueryBreadcrumb, IEnumerable<CategoryListViewDto>>,
        IQueryHandler<CategoryQueryByLevel, IEnumerable<CategoryListViewDto>>,
        IQueryHandler<CategoryQueryCombobox, IEnumerable<CategoryComboboxDto>>,
                                             IQueryHandler<CategoryQueryCheckExist, bool>,
                                             IQueryHandler<CategoryQueryById, CategoryDto>,
                                             IQueryHandler<CategoryQueryByCode, CategoryDto>,
                                             IQueryHandler<CategoryQueryAllParent, IEnumerable<CategoryParentDto>>,
                                             IQueryHandler<CategoryPagingQuery, PagedResult<List<CategoryDto>>>,
                                             IQueryHandler<SearchCategoryQuery, PagingResponse<CategoryDto>>
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryQueryHandler(ICategoryRepository categoryRespository)
        {
            _categoryRepository = categoryRespository;
        }
        public async Task<bool> Handle(CategoryQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _categoryRepository.CheckExistById(request.Id);
        }
        public async Task<IEnumerable<CategoryListViewDto>> Handle(CategoryQueryByLevel request, CancellationToken cancellationToken)
        {


            var filter = new Dictionary<string, object>();
            filter.Add("status", request.Status);

            Guid group = Guid.Empty;
            if (!Guid.TryParse(request.GroupCode, out group))
            {
                filter.Add("group", request.GroupCode);
            }
            else
            {
                filter.Add("groupid", request.GroupCode);
            }
            filter.Add("parent", request.Parent);
            filter.Add("level", request.Level);
            filter.Add("levelCount", request.LevelCount);

            var categorys = await _categoryRepository.Filter("", filter);
            var list = categorys.Select(c => new CategoryListViewDto()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                FullName = c.FullName,
                Description = c.Description,
                Web = c.Web,
                Image = c.Image,
                Url = c.Url,
                ParentCategoryId = c.ParentCategoryId,
                ParentIds = c.ParentIds,
                GroupCategoryId = c.GroupCategoryId,
                DisplayOrder = c.DisplayOrder,
                SourceCode = c.SourceCode,
                SourceLink = c.SourceLink,
                JsonData = c.JsonData,
                Level = c.Level,
                Status = c.Status
            });

            return list;
        }
        public async Task<CategoryDto> Handle(CategoryQueryById request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetById(request.CategoryId);

            var parentCategory = category.ParentCategoryId != null ? await _categoryRepository.GetById((Guid)category.ParentCategoryId) : null;
            var result = new CategoryDto()
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                FullName = category.FullName,
                ParentIds = category.ParentIds,
                Level = category.Level,
                GroupCategoryCode = category.GroupCategoryCode,
                GroupCategoryName = category.GroupCategoryName,
                Description = category.Description,
                Web = category.Web,
                Image = category.Image,
                Url = category.Url,
                GroupCategoryId = category.GroupCategoryId,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = parentCategory != null ? parentCategory.Name : "",
                DisplayOrder = category.DisplayOrder,
                Status = category.Status,
                Keywords = category.Keywords,
                JsonData = category.JsonData,
                SourceCode = category.SourceCode,
                SourceLink = category.SourceLink,
                CreatedBy = category.CreatedBy,
                CreatedDate = category.CreatedDate,
                CreatedByName = category.CreatedByName,
                UpdatedBy = category.UpdatedBy,
                UpdatedDate = category.UpdatedDate,
                UpdatedByName = category.UpdatedByName
            };
            return result;
        }
        public async Task<CategoryDto> Handle(CategoryQueryByCode request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByCode(request.Code, request.Group);

            var parentCategory = category.ParentCategoryId != null ? await _categoryRepository.GetById((Guid)category.ParentCategoryId) : null;
            var result = new CategoryDto()
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                FullName = category.FullName,
                ParentIds = category.ParentIds,
                Level = category.Level,
                GroupCategoryCode = category.GroupCategoryCode,
                GroupCategoryName = category.GroupCategoryName,
                Description = category.Description,
                Web = category.Web,
                Image = category.Image,
                Url = category.Url,
                GroupCategoryId = category.GroupCategoryId,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = parentCategory != null ? parentCategory.Name : "",
                DisplayOrder = category.DisplayOrder,
                SourceCode = category.SourceCode,
                SourceLink = category.SourceLink,
                Status = category.Status,
                Keywords = category.Keywords,
                JsonData = category.JsonData,
                CreatedBy = category.CreatedBy,
                CreatedDate = category.CreatedDate,
                CreatedByName = category.CreatedByName,
                UpdatedBy = category.UpdatedBy,
                UpdatedDate = category.UpdatedDate,
                UpdatedByName = category.UpdatedByName
            };
            return result;
        }
        public async Task<IEnumerable<CategoryParentDto>> Handle(CategoryQueryAllParent request, CancellationToken cancellationToken)
        {
            List<CategoryParentDto> List = new List<CategoryParentDto>();
            var listP = await RecursionParentId(List, request.CategoryId);
            return listP;
        }
        private async Task<List<CategoryParentDto>> RecursionParentId(List<CategoryParentDto> List, Guid? id)
        {
            if (id != null)
            {
                var category = await _categoryRepository.GetById((Guid)id);
                List.Add(new CategoryParentDto { Id = category.Id, Name = category.Name });
                await RecursionParentId(List, category.ParentCategoryId);
            }
            return List;
        }
        public async Task<PagedResult<List<CategoryDto>>> Handle(CategoryPagingQuery request, CancellationToken cancellationToken)
        {
            Dictionary<string, object> filter = new Dictionary<string, object>();
            if (request.Status != null)
            {
                filter.Add("status", request.Status);
            }

            if (!String.IsNullOrEmpty(request.GroupCategoryId))
            {
                filter.Add("groupId", request.GroupCategoryId);
            }

            if (!String.IsNullOrEmpty(request.ParentCategoryId))
            {
                filter.Add("parentId", request.ParentCategoryId);
            }
            var fopRequest = FopExpressionBuilder<Category>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<CategoryDto>>();

            var (datas, count) = await _categoryRepository.Filter(request.Keyword, filter, fopRequest);
            var data = datas.Select(category => new CategoryDto()
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                FullName = category.FullName,
                Description = category.Description,
                Image = category.Image,
                Web = category.Web,
                Url = category.Url,
                ParentCategoryId = category.ParentCategoryId,
                Status = category.Status,
                CreatedByName = category.CreatedByName,
                UpdatedByName = category.UpdatedByName,
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }


        public async Task<IEnumerable<CategoryListBoxDto>> Handle(CategoryQueryListBox request, CancellationToken cancellationToken)
        {

            var categorys = await _categoryRepository.GetListListBox(request.Filter, request.Keyword);
            var result = categorys.Select(data => new CategoryListBoxDto()
            {
                Value = data.Id,
                Label = data.Name,
                Key = data.Code,
                ParentCategoryId = data.ParentCategoryId,
                DisplayOrder = data.DisplayOrder
            });
            return result;
        }
        public async Task<IEnumerable<CategoryComboboxDto>> Handle(CategoryQueryCombobox request, CancellationToken cancellationToken)
        {

            var categorys = await _categoryRepository.GetCombobox(request.Filter, request.Keyword);
            var result = categorys.Select(data => new CategoryComboboxDto()
            {
                Value = data.Id,
                Label = data.FullName,
                Key = data.Code,
                ParentCategoryId = data.ParentCategoryId,
            });
            return result;
        }

        public async Task<IEnumerable<CategoryListViewDto>> Handle(CategoryQueryBreadcrumb request, CancellationToken cancellationToken)
        {
            var categorys = await _categoryRepository.GetBreadcrumb(request.Group, request.Category);
            var list = categorys.Select(c => new CategoryListViewDto()
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                FullName = c.FullName,
                Description = c.Description,
                Web = c.Web,
                Image = c.Image,
                Url = c.Url,
                ParentCategoryId = c.ParentCategoryId,
                ParentIds = c.ParentIds,
                GroupCategoryId = c.GroupCategoryId,
                DisplayOrder = c.DisplayOrder,
                SourceCode = c.SourceCode,
                SourceLink = c.SourceLink,
                JsonData = c.JsonData,
                Level = c.Level,
                Status = c.Status
            });

            return list;
        }

        public async Task<PagingResponse<CategoryDto>> Handle(SearchCategoryQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<CategoryDto>();
            var filter = new Dictionary<string, object>();
            filter.Add("group", request.Group);
            if (request.Status.HasValue) filter.Add("status", request.Status);

            var count = await _categoryRepository.FilterCount(request.Keyword, filter);
            var items = await _categoryRepository.Filter(request.Keyword, filter, request.PageSize, request.PageIndex);
            var data = items.Select(category => new CategoryDto()
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                FullName = category.FullName,
                Description = category.Description,
                Image = category.Image,
                Web = category.Web,
                Url = category.Url,
                ParentCategoryId = category.ParentCategoryId,
                SourceCode = category.SourceCode,
                SourceLink = category.SourceLink,
                Status = category.Status,
                CreatedByName = category.CreatedByName,
                UpdatedByName = category.UpdatedByName,
            });
            response.Items = data;
            response.Total = count;
            response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }
    }
}
