using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Repository;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductAttributeQueryAll : IQuery<IEnumerable<ProductAttributeDto>>
    {
        public ProductAttributeQueryAll()
        {
        }
    }

    public class ProductAttributeQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductAttributeQueryListBox(string? keyword, int? status)
        {
            Keyword = keyword;
            Status = status;
        }
        public string? Keyword { get; set; }
        public int? Status { get; set; }
    }
    public class ProductAttributeQueryById : IQuery<ProductAttributeDto>
    {
        public ProductAttributeQueryById()
        {
        }

        public ProductAttributeQueryById(Guid productAttributeId)
        {
            ProductAttributeId = productAttributeId;
        }

        public Guid ProductAttributeId { get; set; }
    }
    public class ProductAttributeQueryCheckExist : IQuery<bool>
    {
        public ProductAttributeQueryCheckExist(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
    public class ProductAttributePagingQuery : FopQuery, IQuery<PagedResult<List<ProductAttributeDto>>>
    {
        public ProductAttributePagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public string? Keyword { get; set; }
    }

    public class ProductAttributeQueryHandler : IQueryHandler<ProductAttributeQueryListBox, IEnumerable<ListBoxDto>>,
                                            IQueryHandler<ProductAttributeQueryAll, IEnumerable<ProductAttributeDto>>,
                                            IQueryHandler<ProductAttributeQueryById, ProductAttributeDto>,
                                            IQueryHandler<ProductAttributeQueryCheckExist, bool>,
                                            IQueryHandler<ProductAttributePagingQuery, PagedResult<List<ProductAttributeDto>>>
    {
        private readonly IProductAttributeRepository _Attribute;
        private readonly IProductAttributeOptionRepository _Option;
        public ProductAttributeQueryHandler(IProductAttributeRepository Attribute, IProductAttributeOptionRepository Option)
        {
            _Attribute = Attribute;
            _Option = Option;
        }
        public async Task<bool> Handle(ProductAttributeQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _Attribute.CheckExistById(request.Id);
        }
        public async Task<ProductAttributeDto> Handle(ProductAttributeQueryById request, CancellationToken cancellationToken)
        {
            var obj = await _Attribute.GetById(request.ProductAttributeId);
            var option = await _Option.Filter(request.ProductAttributeId);
            var result = new ProductAttributeDto()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                Description = obj.Description,
                Alias = obj.Alias,
                AllowFiltering = obj.AllowFiltering,
                SearchType = obj.SearchType,
                IsOption = obj.IsOption,
                DisplayOrder = obj.DisplayOrder,
                Mapping = obj.Mapping,
                CreatedBy = obj.CreatedBy,
                CreatedDate = obj.CreatedDate,
                CreatedByName = obj.CreatedByName,
                UpdatedBy = obj.UpdatedBy,
                UpdatedDate = obj.UpdatedDate,
                UpdatedByName = obj.UpdatedByName,
                Status = obj.Status,
                Options = option.Select(x => new ProductAttributeOptionDto()
                {
                    Id = x.Id,
                    ProductAttributeId = x.ProductAttributeId,
                    Name = x.Name,
                    Alias = x.Alias,
                    Image = x.Image,
                    Color = x.Color,
                    PriceAdjustment = x.PriceAdjustment,
                    WeightAdjustment = x.WeightAdjustment,
                    IsPreSelected = x.IsPreSelected,
                    DisplayOrder = x.DisplayOrder,
                    ValueTypeId = x.ValueTypeId,
                    LinkedProductId = x.LinkedProductId,
                    Quantity = x.Quantity,
                    CreatedDate = x.CreatedDate,
                    UpdatedDate = x.UpdatedDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    CreatedByName = x.CreatedByName,
                    UpdatedByName = x.UpdatedByName
                }).ToList()
            };
            return result;
        }

        public async Task<PagedResult<List<ProductAttributeDto>>> Handle(ProductAttributePagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<Domain.PIM.Models.ProductAttribute>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<ProductAttributeDto>>();
            var (datas, count) = await _Attribute.Filter(request.Keyword, fopRequest);
            var data = datas.Select(obj => new ProductAttributeDto()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                Description = obj.Description,
                Alias = obj.Alias,
                AllowFiltering = obj.AllowFiltering,
                SearchType = obj.SearchType,
                IsOption = obj.IsOption,
                DisplayOrder = obj.DisplayOrder,
                Mapping = obj.Mapping,
                CreatedBy = obj.CreatedBy,
                CreatedDate = obj.CreatedDate,
                UpdatedBy = obj.UpdatedBy,
                UpdatedDate = obj.UpdatedDate,
                Status = obj.Status
            }).OrderBy(x => x.DisplayOrder).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ProductAttributeDto>> Handle(ProductAttributeQueryAll request, CancellationToken cancellationToken)
        {
            var data = await _Attribute.GetAll();
            var result = data.Select(obj => new ProductAttributeDto()
            {
                Id = obj.Id,
                Code = obj.Code,
                Name = obj.Name,
                Description = obj.Description,
                Alias = obj.Alias,
                AllowFiltering = obj.AllowFiltering,
                SearchType = obj.SearchType,
                IsOption = obj.IsOption,
                DisplayOrder = obj.DisplayOrder,
                Mapping = obj.Mapping,
                CreatedBy = obj.CreatedBy,
                CreatedDate = obj.CreatedDate,
                UpdatedBy = obj.UpdatedBy,
                UpdatedDate = obj.UpdatedDate,
                Status = obj.Status
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductAttributeQueryListBox request, CancellationToken cancellationToken)
        {

            var ProductAttributes = await _Attribute.GetListListBox(request.Keyword, request.Status);
            var result = ProductAttributes.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
