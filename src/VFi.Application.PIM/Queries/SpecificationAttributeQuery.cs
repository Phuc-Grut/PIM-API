using Consul;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace VFi.Application.PIM.Queries
{

    public class SpecificationAttributeQueryAll : IQuery<IEnumerable<SpecificationAttributeDto>>
    {
        public SpecificationAttributeQueryAll()
        {
        }
    }

    public class SpecificationAttributeQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public SpecificationAttributeQueryListBox(string? keyword, int? status)
        {
            Keyword = keyword;
            Status = status;
        }
        public string? Keyword { get; set; }
        public int? Status { get; set; }
    }
    public class SpecificationAttributeQueryById : IQuery<SpecificationAttributeDto>
    {
        public SpecificationAttributeQueryById()
        {
        }

        public SpecificationAttributeQueryById(Guid specificationAttributeId)
        {
            SpecificationAttributeId = specificationAttributeId;
        }

        public Guid SpecificationAttributeId { get; set; }
    }
    public class SpecificationAttributeQueryCheckExist : IQuery<bool>
    {
        public SpecificationAttributeQueryCheckExist(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
    public class SpecificationAttributePagingQuery : FopQuery, IQuery<PagedResult<List<SpecificationAttributeDto>>>
    {
        public SpecificationAttributePagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public string? Keyword { get; set; }
    }

    public class SpecificationAttributeQueryHandler : IQueryHandler<SpecificationAttributeQueryListBox, IEnumerable<ListBoxDto>>,
                                            IQueryHandler<SpecificationAttributeQueryAll, IEnumerable<SpecificationAttributeDto>>,
                                            IQueryHandler<SpecificationAttributeQueryById, SpecificationAttributeDto>,
                                            IQueryHandler<SpecificationAttributeQueryCheckExist, bool>,
                                            IQueryHandler<SpecificationAttributePagingQuery, PagedResult<List<SpecificationAttributeDto>>>
    {
        private readonly ISpecificationAttributeRepository _Attribute;
        private readonly ISpecificationAttributeOptionRepository _Option;
        public SpecificationAttributeQueryHandler(ISpecificationAttributeRepository Attribute, ISpecificationAttributeOptionRepository Option)
        {
            _Attribute = Attribute;
            _Option = Option;
        }
        public async Task<bool> Handle(SpecificationAttributeQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _Attribute.CheckExistById(request.Id);
        }
        public async Task<SpecificationAttributeDto> Handle(SpecificationAttributeQueryById request, CancellationToken cancellationToken)
        {
            var obj = await _Attribute.GetById(request.SpecificationAttributeId);
            var option = await _Option.Filter(request.SpecificationAttributeId);
            var result = new SpecificationAttributeDto()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                Alias = obj.Alias,
                Description = obj.Description,
                Status = obj.Status,
                DisplayOrder = obj.DisplayOrder,
                CreatedDate = obj.CreatedDate,
                UpdatedDate = obj.UpdatedDate,
                CreatedBy = obj.CreatedBy,
                UpdatedBy = obj.UpdatedBy,
                CreatedByName = obj.CreatedByName,
                UpdatedByName = obj.UpdatedByName,
                Options = option.Select(x => new SpecificationAttributeOptionDto()
                {
                    Id = x.Id,
                    SpecificationAttributeId = x.SpecificationAttributeId,
                    Name = x.Name,
                    Code = x.Code,
                    NumberValue = x.NumberValue,
                    Color = x.Color,
                    DisplayOrder = x.DisplayOrder,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy
                }).ToList()
            };
            return result;
        }

        public async Task<PagedResult<List<SpecificationAttributeDto>>> Handle(SpecificationAttributePagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<SpecificationAttribute>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<SpecificationAttributeDto>>();
            var (datas, count) = await _Attribute.Filter(request.Keyword, fopRequest);
            var data = datas.Select(obj => new SpecificationAttributeDto()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                Alias = obj.Alias,
                Description = obj.Description,
                Status = obj.Status,
                DisplayOrder = obj.DisplayOrder,
                CreatedDate = obj.CreatedDate,
                UpdatedDate = obj.UpdatedDate,
                CreatedBy = obj.CreatedBy,
                UpdatedBy = obj.UpdatedBy,
                CreatedByName = obj.CreatedByName,
                UpdatedByName = obj.UpdatedByName
            }).OrderBy(x => x.DisplayOrder).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<SpecificationAttributeDto>> Handle(SpecificationAttributeQueryAll request, CancellationToken cancellationToken)
        {
            var data = await _Attribute.GetAll();
            var result = data.Select(obj => new SpecificationAttributeDto()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                Alias = obj.Alias,
                Description = obj.Description,
                Status = obj.Status,
                DisplayOrder = obj.DisplayOrder,
                CreatedDate = obj.CreatedDate,
                UpdatedDate = obj.UpdatedDate,
                CreatedBy = obj.CreatedBy,
                UpdatedBy = obj.UpdatedBy
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(SpecificationAttributeQueryListBox request, CancellationToken cancellationToken)
        {

            var SpecificationAttributes = await _Attribute.GetListListBox(request.Keyword, request.Status);
            var result = SpecificationAttributes.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
