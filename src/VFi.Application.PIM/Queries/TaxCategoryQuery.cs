using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{
    
    public class TaxCategoryQueryAll : IQuery<IEnumerable<TaxCategoryDto>>
    {
        public TaxCategoryQueryAll()
        {
        }
    }

    public class TaxCategoryQueryListBox : IQuery<IEnumerable<ListTaxDto>>
    {
        public TaxCategoryQueryListBox(int? status, string? keyword)
        {
            Keyword = keyword;
            Status = status;
        }
        public string? Keyword { get; set; }
        public int? Status { get; set; }
    }
    public class TaxCategoryQueryCheckExist : IQuery<bool>
    {

        public TaxCategoryQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class TaxCategoryQueryById : IQuery<TaxCategoryDto>
    {
        public TaxCategoryQueryById()
        {
        }

        public TaxCategoryQueryById(Guid taxCategoryId)
        {
            TaxCategoryId = taxCategoryId;
        }

        public Guid TaxCategoryId { get; set; }
    }
    public class TaxCategoryPagingQuery : FopQuery, IQuery<PagedResult<List<TaxCategoryDto>>>
    {
        public TaxCategoryPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string? Keyword { get; set; }
    }

    public class TaxCategoryQueryHandler : IQueryHandler<TaxCategoryQueryListBox, IEnumerable<ListTaxDto>>, 
                                             IQueryHandler<TaxCategoryQueryAll, IEnumerable<TaxCategoryDto>>, 
                                             IQueryHandler<TaxCategoryQueryCheckExist, bool>,
                                             IQueryHandler<TaxCategoryQueryById, TaxCategoryDto>, 
                                             IQueryHandler<TaxCategoryPagingQuery, PagedResult<List<TaxCategoryDto>>>
    {
        private readonly ITaxCategoryRepository _taxCategoryRepository;
        public TaxCategoryQueryHandler(ITaxCategoryRepository taxCategoryRespository)
        {
            _taxCategoryRepository = taxCategoryRespository;
        }
        public async Task<bool> Handle(TaxCategoryQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _taxCategoryRepository.CheckExistById(request.Id);
        }

        public async Task<TaxCategoryDto> Handle(TaxCategoryQueryById request, CancellationToken cancellationToken)
        {
            var taxCategory = await _taxCategoryRepository.GetById(request.TaxCategoryId);
            var result = new TaxCategoryDto()
            {
                Id = taxCategory.Id,
                Code = taxCategory.Code,
                Name = taxCategory.Name,
                Rate = taxCategory.Rate,
                Group = taxCategory.Group,
                Description = taxCategory.Description,
                DisplayOrder = taxCategory.DisplayOrder,
                Status = taxCategory.Status,
                Type = taxCategory.Type,
                CreatedBy = taxCategory.CreatedBy,
                CreatedDate = taxCategory.CreatedDate,
                CreatedByName = taxCategory.CreatedByName,
                UpdatedBy = taxCategory.UpdatedBy,
                UpdatedDate = taxCategory.UpdatedDate,
                UpdatedByName = taxCategory.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<TaxCategoryDto>>> Handle(TaxCategoryPagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<TaxCategory>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<TaxCategoryDto>>();
            var (datas, count) = await _taxCategoryRepository.Filter(request.Keyword, fopRequest);
            var data = datas.Select(taxCategory => new TaxCategoryDto()
            {
                Id = taxCategory.Id,
                Code = taxCategory.Code,
                Name = taxCategory.Name,
                Rate = taxCategory.Rate,
                Group = taxCategory.Group,
                Description = taxCategory.Description,
                DisplayOrder = taxCategory.DisplayOrder,
                Status = taxCategory.Status,
                Type = taxCategory.Type,
                CreatedBy = taxCategory.CreatedBy,
                CreatedDate = taxCategory.CreatedDate,
                CreatedByName = taxCategory.CreatedByName,
                UpdatedBy = taxCategory.UpdatedBy,
                UpdatedDate = taxCategory.UpdatedDate,
                UpdatedByName = taxCategory.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<TaxCategoryDto>> Handle(TaxCategoryQueryAll request, CancellationToken cancellationToken)
        {
            var taxCategorys = await _taxCategoryRepository.GetAll();
            var result = taxCategorys.Select(taxCategory => new TaxCategoryDto()
            {
                Id = taxCategory.Id,
                Code = taxCategory.Code,
                Name = taxCategory.Name,
                Rate = taxCategory.Rate,
                Group = taxCategory.Group,
                Description = taxCategory.Description,
                DisplayOrder = taxCategory.DisplayOrder,
                Status = taxCategory.Status,
                CreatedBy = taxCategory.CreatedBy,
                CreatedDate = taxCategory.CreatedDate,
                UpdatedBy = taxCategory.UpdatedBy,
                UpdatedDate = taxCategory.UpdatedDate
            });
            return result;
        }
        
        public async Task<IEnumerable<ListTaxDto>> Handle(TaxCategoryQueryListBox request, CancellationToken cancellationToken)
        {

            var taxCategorys = await _taxCategoryRepository.GetListListBox(request.Status, request.Keyword);
            var result = taxCategorys.Select(x => new ListTaxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name,
                Rate= x.Rate
            });
            return result;
        }
    }
}
