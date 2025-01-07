using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class SpecificationAttributeOptionQueryAll : IQuery<IEnumerable<SpecificationAttributeOptionDto>>
    {
        public SpecificationAttributeOptionQueryAll()
        {
        }
    }

    //public class SpecificationAttributeOptionQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    //{
    //    public SpecificationAttributeOptionQueryListBox(string? specificationAttributeId, string? keyword)
    //    {
    //        Keyword = keyword;
    //        Filter = new Dictionary<string, object>();
    //        if (!String.IsNullOrEmpty(specificationAttributeId))
    //        {
    //            Filter.Add("specificationAttributeId", specificationAttributeId);
    //        }
    //    }
    //    public string? Keyword { get; set; }
    //    public Dictionary<string, object> Filter
    //    {
    //        get; set;
    //    }
    //}
    //public class SpecificationAttributeOptionQueryById : IQuery<SpecificationAttributeOptionDto>
    //{
    //    public SpecificationAttributeOptionQueryById()
    //    {
    //    }

    //    public SpecificationAttributeOptionQueryById(Guid specificationAttributeOptionId)
    //    {
    //        SpecificationAttributeOptionId = specificationAttributeOptionId;
    //    }

    //    public Guid SpecificationAttributeOptionId { get; set; }
    //}
    //public class SpecificationAttributeOptionQueryCheckExist : IQuery<bool>
    //{
    //    public SpecificationAttributeOptionQueryCheckExist(Guid id)
    //    {
    //        Id = id;
    //    }

    //    public Guid Id { get; set; }
    //}
    //public class SpecificationAttributeOptionPagingQuery : ListQuery, IQuery<PagingResponse<SpecificationAttributeOptionDto>>
    //{
    //    public SpecificationAttributeOptionPagingQuery(string? keyword, string? specificationAttributeId, int pageSize, int pageIndex) : base(pageSize, pageIndex)
    //    {
    //        Keyword = keyword;
    //        Filter = new Dictionary<string, object>();
    //        if (!String.IsNullOrEmpty(specificationAttributeId))
    //        {
    //            Filter.Add("specificationAttributeId", specificationAttributeId);
    //        }
    //    }

    //    public SpecificationAttributeOptionPagingQuery(string? keyword, string? specificationAttributeId, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex)
    //    {
    //        Keyword = keyword;
    //        Filter = new Dictionary<string, object>();
    //        if (!String.IsNullOrEmpty(specificationAttributeId))
    //        {
    //            Filter.Add("specificationAttributeId", specificationAttributeId);
    //        }
    //    }
    //    public string? Keyword { get; set; }
    //    public Dictionary<string, object> Filter
    //    {
    //        get; set;
    //    }
    //}

    public class SpecificationAttributeOptionQueryHandler : 
                                        IQueryHandler<SpecificationAttributeOptionQueryAll, IEnumerable<SpecificationAttributeOptionDto>>
                                        //IQueryHandler<SpecificationAttributeOptionQueryListBox, IEnumerable<ListBoxDto>>,
                                        //IQueryHandler<SpecificationAttributeOptionQueryById, SpecificationAttributeOptionDto>,
                                        //IQueryHandler<SpecificationAttributeOptionQueryCheckExist, bool>,
                                        //IQueryHandler<SpecificationAttributeOptionPagingQuery, PagingResponse<SpecificationAttributeOptionDto>>
    {
        private readonly ISpecificationAttributeOptionRepository _SpecificationAttributeOptionRepository;
        public SpecificationAttributeOptionQueryHandler(ISpecificationAttributeOptionRepository SpecificationAttributeOptionRespository)
        {
            _SpecificationAttributeOptionRepository = SpecificationAttributeOptionRespository;
        }
        //public async Task<bool> Handle(SpecificationAttributeOptionQueryCheckExist request, CancellationToken cancellationToken)
        //{
        //    return await _SpecificationAttributeOptionRepository.CheckExistById(request.Id);
        //}
        //public async Task<SpecificationAttributeOptionDto> Handle(SpecificationAttributeOptionQueryById request, CancellationToken cancellationToken)
        //{
        //    var SpecificationAttributeOption = await _SpecificationAttributeOptionRepository.GetById(request.SpecificationAttributeOptionId);
        //    var result = new SpecificationAttributeOptionDto()
        //    {
        //        Id = SpecificationAttributeOption.Id,
        //        Name = SpecificationAttributeOption.Name,
        //        Alias = SpecificationAttributeOption.Alias,
        //        Color = SpecificationAttributeOption.Color,
        //        MediaFileId = SpecificationAttributeOption.MediaFileId,
        //        NumberValue = SpecificationAttributeOption.NumberValue,
        //        SpecificationAttributeId = SpecificationAttributeOption.SpecificationAttributeId,
        //        DisplayOrder = SpecificationAttributeOption.DisplayOrder,
        //        CreatedBy = SpecificationAttributeOption.CreatedBy,
        //        CreatedDate = SpecificationAttributeOption.CreatedDate,
        //        UpdatedBy = SpecificationAttributeOption.UpdatedBy,
        //        UpdatedDate = SpecificationAttributeOption.UpdatedDate
        //    };
        //    return result;
        //}

        //public async Task<PagingResponse<SpecificationAttributeOptionDto>> Handle(SpecificationAttributeOptionPagingQuery request, CancellationToken cancellationToken)
        //{
        //    var response = new PagingResponse<SpecificationAttributeOptionDto>();
        //    var count = await _SpecificationAttributeOptionRepository.FilterCount(request.Keyword, request.Filter);
        //    var SpecificationAttributeOptions = await _SpecificationAttributeOptionRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
        //    var data = SpecificationAttributeOptions.Select(SpecificationAttributeOption => new SpecificationAttributeOptionDto()
        //    {
        //        Id = SpecificationAttributeOption.Id,
        //        Name = SpecificationAttributeOption.Name,
        //        Alias = SpecificationAttributeOption.Alias,
        //        Color = SpecificationAttributeOption.Color,
        //        MediaFileId = SpecificationAttributeOption.MediaFileId,
        //        NumberValue = SpecificationAttributeOption.NumberValue,
        //        SpecificationAttributeId = SpecificationAttributeOption.SpecificationAttributeId,
        //        DisplayOrder = SpecificationAttributeOption.DisplayOrder,
        //        CreatedBy = SpecificationAttributeOption.CreatedBy,
        //        CreatedDate = SpecificationAttributeOption.CreatedDate,
        //        UpdatedBy = SpecificationAttributeOption.UpdatedBy,
        //        UpdatedDate = SpecificationAttributeOption.UpdatedDate
        //    });
        //    response.Items = data;
        //    response.Total = count; response.Count = count;
        //    response.PageIndex = request.PageIndex;
        //    response.PageSize = request.PageSize;
        //    response.Successful();
        //    return response;
        //}

        public async Task<IEnumerable<SpecificationAttributeOptionDto>> Handle(SpecificationAttributeOptionQueryAll request, CancellationToken cancellationToken)
        {
            var data = await _SpecificationAttributeOptionRepository.GetAll();
            var result = data.Select(x => new SpecificationAttributeOptionDto()
            {
                Id = x.Id,
                Name = x.Name,
                Color = x.Color,
                NumberValue = x.NumberValue,
                SpecificationAttributeId = x.SpecificationAttributeId,
                DisplayOrder = x.DisplayOrder,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                Code = x.Code,
            });
            return result;
        }

        //public async Task<IEnumerable<ListBoxDto>> Handle(SpecificationAttributeOptionQueryListBox request, CancellationToken cancellationToken)
        //{

        //    var SpecificationAttributeOptions = await _SpecificationAttributeOptionRepository.GetListListBox(request.Filter, request.Keyword);
        //    var result = SpecificationAttributeOptions.Select(x => new ListBoxDto()
        //    {
        //        Value = x.Id,
        //        Label = x.Name
        //    });
        //    return result;
        //}
    }
}
