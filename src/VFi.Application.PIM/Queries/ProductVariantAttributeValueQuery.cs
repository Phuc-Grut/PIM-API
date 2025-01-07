using Consul.Filtering;
using Consul;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductVariantAttributeValueQueryAll : IQuery<IEnumerable<ProductVariantAttributeValueDto>>
    {
        public ProductVariantAttributeValueQueryAll()
        {
        }
    }

    public class ProductVariantAttributeValueQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductVariantAttributeValueQueryListBox(string? productVariantAttributeId, string? keyword)
        {
            Keyword = keyword;
            Filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(productVariantAttributeId))
            {
                Filter.Add("productVariantAttributeId", productVariantAttributeId);
            }
        }
        public string? Keyword { get; set; }
        public Dictionary<string, object> Filter
        {
            get; set;
        }
    }
    public class ProductVariantAttributeValueQueryById : IQuery<ProductVariantAttributeValueDto>
    {
        public ProductVariantAttributeValueQueryById()
        {
        }

        public ProductVariantAttributeValueQueryById(Guid productVariantAttributeValueId)
        {
            ProductVariantAttributeValueId = productVariantAttributeValueId;
        }

        public Guid ProductVariantAttributeValueId { get; set; }
    }
    public class ProductVariantAttributeValueQueryCheckExist : IQuery<bool>
    {
        public ProductVariantAttributeValueQueryCheckExist(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
    public class ProductVariantAttributeValuePagingQuery : ListQuery, IQuery<PagingResponse<ProductVariantAttributeValueDto>>
    {
        public ProductVariantAttributeValuePagingQuery(string? keyword, string? productVariantAttributeId, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
            Filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(productVariantAttributeId))
            {
                Filter.Add("productVariantAttributeId", productVariantAttributeId);
            }
        }

        public ProductVariantAttributeValuePagingQuery(string? keyword, string? productVariantAttributeId, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
            Filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(productVariantAttributeId))
            {
                Filter.Add("productVariantAttributeId", productVariantAttributeId);
            }
        }
        public string? Keyword { get; set; }
        public Dictionary<string, object> Filter
        {
            get; set;
        }
    }

    public class ProductVariantAttributeValueQueryHandler : IQueryHandler<ProductVariantAttributeValueQueryListBox, IEnumerable<ListBoxDto>>,
                                            IQueryHandler<ProductVariantAttributeValueQueryAll, IEnumerable<ProductVariantAttributeValueDto>>,
                                            IQueryHandler<ProductVariantAttributeValueQueryById, ProductVariantAttributeValueDto>,
                                            IQueryHandler<ProductVariantAttributeValueQueryCheckExist, bool>,
                                            IQueryHandler<ProductVariantAttributeValuePagingQuery, PagingResponse<ProductVariantAttributeValueDto>>
    {
        private readonly IProductVariantAttributeValueRepository _productVariantAttributeValueRepository;
        public ProductVariantAttributeValueQueryHandler(IProductVariantAttributeValueRepository productVariantAttributeValueRespository)
        {
            _productVariantAttributeValueRepository = productVariantAttributeValueRespository;
        }
        public async Task<bool> Handle(ProductVariantAttributeValueQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productVariantAttributeValueRepository.CheckExistById(request.Id);
        }
        public async Task<ProductVariantAttributeValueDto> Handle(ProductVariantAttributeValueQueryById request, CancellationToken cancellationToken)
        {
            var productVariantAttributeValue = await _productVariantAttributeValueRepository.GetById(request.ProductVariantAttributeValueId);
            var result = new ProductVariantAttributeValueDto()
            {
                Id = productVariantAttributeValue.Id,
                ProductVariantAttributeId = productVariantAttributeValue.ProductVariantAttributeId,
                Code = productVariantAttributeValue.Code,
                Name = productVariantAttributeValue.Name,
                Alias = productVariantAttributeValue.Alias,
                Image = productVariantAttributeValue.Image,
                Color = productVariantAttributeValue.Color,
                PriceAdjustment = productVariantAttributeValue.PriceAdjustment,
                WeightAdjustment = productVariantAttributeValue.WeightAdjustment,
                DisplayOrder = productVariantAttributeValue.DisplayOrder,
                CreatedBy = productVariantAttributeValue.CreatedBy,
                CreatedDate = productVariantAttributeValue.CreatedDate,
                UpdatedBy = productVariantAttributeValue.UpdatedBy,
                UpdatedDate = productVariantAttributeValue.UpdatedDate
            };
            return result;
        }

        public async Task<PagingResponse<ProductVariantAttributeValueDto>> Handle(ProductVariantAttributeValuePagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductVariantAttributeValueDto>();
            var count = await _productVariantAttributeValueRepository.FilterCount(request.Keyword, request.Filter);
            var productVariantAttributeValue = await _productVariantAttributeValueRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
            var data = productVariantAttributeValue.Select(productVariantAttributeValue => new ProductVariantAttributeValueDto()
            {
                Id = productVariantAttributeValue.Id,
                ProductVariantAttributeId = productVariantAttributeValue.ProductVariantAttributeId,
                Code = productVariantAttributeValue.Code,
                Name = productVariantAttributeValue.Name,
                Alias = productVariantAttributeValue.Alias,
                Image = productVariantAttributeValue.Image,
                Color = productVariantAttributeValue.Color,
                PriceAdjustment = productVariantAttributeValue.PriceAdjustment,
                WeightAdjustment = productVariantAttributeValue.WeightAdjustment,
                DisplayOrder = productVariantAttributeValue.DisplayOrder,
                CreatedBy = productVariantAttributeValue.CreatedBy,
                CreatedDate = productVariantAttributeValue.CreatedDate,
                UpdatedBy = productVariantAttributeValue.UpdatedBy,
                UpdatedDate = productVariantAttributeValue.UpdatedDate
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductVariantAttributeValueDto>> Handle(ProductVariantAttributeValueQueryAll request, CancellationToken cancellationToken)
        {
            var productVariantAttributeValue = await _productVariantAttributeValueRepository.GetAll();
            var result = productVariantAttributeValue.Select(productVariantAttributeValue => new ProductVariantAttributeValueDto()
            {
                Id = productVariantAttributeValue.Id,
                ProductVariantAttributeId = productVariantAttributeValue.ProductVariantAttributeId,
                Code = productVariantAttributeValue.Code,
                Name = productVariantAttributeValue.Name,
                Alias = productVariantAttributeValue.Alias,
                Image = productVariantAttributeValue.Image,
                Color = productVariantAttributeValue.Color,
                PriceAdjustment = productVariantAttributeValue.PriceAdjustment,
                WeightAdjustment = productVariantAttributeValue.WeightAdjustment,
                DisplayOrder = productVariantAttributeValue.DisplayOrder,
                CreatedBy = productVariantAttributeValue.CreatedBy,
                CreatedDate = productVariantAttributeValue.CreatedDate,
                UpdatedBy = productVariantAttributeValue.UpdatedBy,
                UpdatedDate = productVariantAttributeValue.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductVariantAttributeValueQueryListBox request, CancellationToken cancellationToken)
        {

            var productVariantAttributeValue = await _productVariantAttributeValueRepository.GetListListBox(request.Filter, request.Keyword);
            var result = productVariantAttributeValue.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
