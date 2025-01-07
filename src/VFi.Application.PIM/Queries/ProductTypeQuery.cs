using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Queries
{
    public class ProductTypeQueryAll : IQuery<IEnumerable<ProductTypeDto>>
    {
        public ProductTypeQueryAll()
        {
        }
    }

    public class ProductTypeQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductTypeQueryListBox(int? status, string? keyword)
        {
            Keyword = keyword;
            Status = status;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
    }
    public class ProductTypeQueryCheckExist : IQuery<bool>
    {

        public ProductTypeQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductTypeQueryById : IQuery<ProductTypeDto>
    {
        public ProductTypeQueryById()
        {
        }

        public ProductTypeQueryById(Guid productTypeId)
        {
            ProductTypeId = productTypeId;
        }

        public Guid ProductTypeId { get; set; }
    }
    public class ProductTypePagingQuery : FopQuery, IQuery<PagedResult<List<ProductTypeDto>>>
    {
        public ProductTypePagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public string? Keyword { get; set; }
    }

    public class ProductTypeQueryHandler : IQueryHandler<ProductTypeQueryListBox, IEnumerable<ListBoxDto>>,
                                             IQueryHandler<ProductTypeQueryAll, IEnumerable<ProductTypeDto>>,
                                             IQueryHandler<ProductTypeQueryCheckExist, bool>,
                                             IQueryHandler<ProductTypeQueryById, ProductTypeDto>,
                                             IQueryHandler<ProductTypePagingQuery, PagedResult<List<ProductTypeDto>>>
    {
        private readonly IProductTypeRepository _productTypeRepository;
        public ProductTypeQueryHandler(IProductTypeRepository productTypeRespository)
        {
            _productTypeRepository = productTypeRespository;
        }
        public async Task<bool> Handle(ProductTypeQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productTypeRepository.CheckExistById(request.Id);
        }

        public async Task<ProductTypeDto> Handle(ProductTypeQueryById request, CancellationToken cancellationToken)
        {
            var productType = await _productTypeRepository.GetById(request.ProductTypeId);
            var result = new ProductTypeDto()
            {
                Id = productType.Id,
                Code = productType.Code,
                Name = productType.Name,
                Description = productType.Description,
                DisplayOrder = productType.DisplayOrder,
                Status = productType.Status,
                CreatedBy = productType.CreatedBy,
                CreatedDate = productType.CreatedDate,
                CreatedByName = productType.CreatedByName,
                UpdatedBy = productType.UpdatedBy,
                UpdatedDate = productType.UpdatedDate,
                UpdatedByName = productType.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<ProductTypeDto>>> Handle(ProductTypePagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<ProductType>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<ProductTypeDto>>();
            var (datas, count) = await _productTypeRepository.Filter(request.Keyword, fopRequest);
            var data = datas.Select(productType => new ProductTypeDto()
            {
                Id = productType.Id,
                Code = productType.Code,
                Name = productType.Name,
                Description = productType.Description,
                DisplayOrder = productType.DisplayOrder,
                Status = productType.Status,
                CreatedBy = productType.CreatedBy,
                CreatedDate = productType.CreatedDate,
                CreatedByName = productType.CreatedByName,
                UpdatedBy = productType.UpdatedBy,
                UpdatedDate = productType.UpdatedDate,
                UpdatedByName = productType.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ProductTypeDto>> Handle(ProductTypeQueryAll request, CancellationToken cancellationToken)
        {
            var productTypes = await _productTypeRepository.GetAll();
            var result = productTypes.Select(productType => new ProductTypeDto()
            {
                Id = productType.Id,
                Code = productType.Code,
                Name = productType.Name,
                Description = productType.Description,
                DisplayOrder = productType.DisplayOrder,
                Status = productType.Status,
                CreatedBy = productType.CreatedBy,
                CreatedDate = productType.CreatedDate,
                UpdatedBy = productType.UpdatedBy,
                UpdatedDate = productType.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductTypeQueryListBox request, CancellationToken cancellationToken)
        {

            var productTypes = await _productTypeRepository.GetListListBox(request.Status, request.Keyword);
            var result = productTypes.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
