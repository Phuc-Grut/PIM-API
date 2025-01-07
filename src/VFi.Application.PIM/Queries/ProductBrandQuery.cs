using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductBrandQueryAll : IQuery<IEnumerable<ProductBrandDto>>
    {
        public ProductBrandQueryAll()
        {
        }
    }

    public class ProductBrandQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductBrandQueryListBox(int? status, string? keyword)
        {
            Status = status;
            Keyword = keyword;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
    }
    public class ProductBrandQueryCheckExist : IQuery<bool>
    {

        public ProductBrandQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductBrandQueryById : IQuery<ProductBrandDto>
    {
        public ProductBrandQueryById()
        {
        }

        public ProductBrandQueryById(Guid productBrandId)
        {
            ProductBrandId = productBrandId;
        }

        public Guid ProductBrandId { get; set; }
    }
    public class ProductBrandPagingQuery : FopQuery, IQuery<PagedResult<List<ProductBrandDto>>>
    {
        public ProductBrandPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
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

    public class ProductBrandQueryHandler : IQueryHandler<ProductBrandQueryListBox, IEnumerable<ListBoxDto>>, 
                                             IQueryHandler<ProductBrandQueryAll, IEnumerable<ProductBrandDto>>, 
                                             IQueryHandler<ProductBrandQueryCheckExist, bool>,
                                             IQueryHandler<ProductBrandQueryById, ProductBrandDto>,
                                             IQueryHandler<ProductBrandPagingQuery, PagedResult<List<ProductBrandDto>>>
    {
        private readonly IProductBrandRepository _productBrandRepository;
        public ProductBrandQueryHandler(IProductBrandRepository productBrandRespository)
        {
            _productBrandRepository = productBrandRespository;
        }
        public async Task<bool> Handle(ProductBrandQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productBrandRepository.CheckExistById(request.Id);
        }

        public async Task<ProductBrandDto> Handle(ProductBrandQueryById request, CancellationToken cancellationToken)
        {
            var productBrand = await _productBrandRepository.GetById(request.ProductBrandId);
            var result = new ProductBrandDto()
            {
                Id = productBrand.Id,
                Code = productBrand.Code,
                Name = productBrand.Name,
                Description= productBrand.Description,
                DisplayOrder = productBrand.DisplayOrder,
                Image = productBrand.Image,
                Tags = productBrand.Tags,
                Status = productBrand.Status,
                CreatedBy = productBrand.CreatedBy,
                CreatedDate = productBrand.CreatedDate,
                CreatedByName = productBrand.CreatedByName,
                UpdatedBy = productBrand.UpdatedBy,
                UpdatedDate = productBrand.UpdatedDate,
                UpdatedByName = productBrand.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<ProductBrandDto>>> Handle(ProductBrandPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<ProductBrandDto>>();
            var fopRequest = FopExpressionBuilder<ProductBrand>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (productBrands, count) = await _productBrandRepository.Filter(request.Keyword, fopRequest);
            var data = productBrands.Select(productBrand => new ProductBrandDto()
            {
                Id = productBrand.Id,
                Code = productBrand.Code,
                Name = productBrand.Name,
                Description = productBrand.Description,
                DisplayOrder = productBrand.DisplayOrder,
                Image = productBrand.Image,
                Tags = productBrand.Tags,
                Status = productBrand.Status,
                CreatedBy = productBrand.CreatedBy,
                CreatedDate = productBrand.CreatedDate,
                CreatedByName = productBrand.CreatedByName,
                UpdatedBy = productBrand.UpdatedBy,
                UpdatedDate = productBrand.UpdatedDate,
                UpdatedByName = productBrand.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ProductBrandDto>> Handle(ProductBrandQueryAll request, CancellationToken cancellationToken)
        {
            var productBrands = await _productBrandRepository.GetAll();
            var result = productBrands.Select(productBrand => new ProductBrandDto()
            {
                Id = productBrand.Id,
                Code = productBrand.Code,
                Name = productBrand.Name,
                Description = productBrand.Description,
                DisplayOrder = productBrand.DisplayOrder,
                Image = productBrand.Image,
                Tags = productBrand.Tags,
                Status = productBrand.Status,
                CreatedBy = productBrand.CreatedBy,
                CreatedDate = productBrand.CreatedDate,
                UpdatedBy = productBrand.UpdatedBy,
                UpdatedDate = productBrand.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductBrandQueryListBox request, CancellationToken cancellationToken)
        {

            var productBrands = await _productBrandRepository.GetListListBox(request.Status, request.Keyword);
            var result = productBrands.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
