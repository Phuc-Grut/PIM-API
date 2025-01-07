using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductPackageQueryAll : IQuery<IEnumerable<ProductPackageDto>>
    {
        public ProductPackageQueryAll()
        {
        }
    }

    public class ProductPackageQueryById : IQuery<ProductPackageDto>
    {
        public ProductPackageQueryById()
        {
        }

        public ProductPackageQueryById(Guid productPackageId)
        {
            ProductPackageId = productPackageId;
        }

        public Guid ProductPackageId { get; set; }
    }
    public class ProductPackageQueryCheckExist : IQuery<bool>
    {

        public ProductPackageQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductPackageQueryByProducts : IQuery<List<ProductPackageDto>>
    {
        public ProductPackageQueryByProducts(string productList)
        {
            ProductList = productList;
        }

        public string ProductList { get; set; }
    }
    public class ProductPackagePagingQuery : ListQuery, IQuery<PagingResponse<ProductPackageDto>>
    {
        public ProductPackagePagingQuery(ProductPackageQueryParams productPackageQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productPackageQueryParams;
        }

        public ProductPackagePagingQuery( ProductPackageQueryParams productPackageQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productPackageQueryParams;
        }

        public ProductPackageQueryParams QueryParams { get; set; }
    }

    public class ProductPackageQueryHandler :
                                             IQueryHandler<ProductPackageQueryAll, IEnumerable<ProductPackageDto>>,
                                             IQueryHandler<ProductPackageQueryCheckExist, bool>,
                                             IQueryHandler<ProductPackageQueryById, ProductPackageDto>,
                                             IQueryHandler<ProductPackagePagingQuery, PagingResponse<ProductPackageDto>>,
                                             IQueryHandler<ProductPackageQueryByProducts, List<ProductPackageDto>>
    {
        private readonly IProductPackageRepository _productPackageRepository;
        public ProductPackageQueryHandler(IProductPackageRepository productPackageRespository)
        {
            _productPackageRepository = productPackageRespository;
        }
        public async Task<bool> Handle(ProductPackageQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productPackageRepository.CheckExistById(request.Id);
        }
        public async Task<ProductPackageDto> Handle(ProductPackageQueryById request, CancellationToken cancellationToken)
        {
            var productPackage = await _productPackageRepository.GetById(request.ProductPackageId);
            var result = new ProductPackageDto()
            {
                Id = productPackage.Id,
                ProductId = productPackage.ProductId,
                Name = productPackage.Name,
                Weight = productPackage.Weight,
                Length = productPackage.Length,
                Width = productPackage.Width,
                Height = productPackage.Height,
                CreatedDate = productPackage.CreatedDate,
                CreatedBy = productPackage.CreatedBy,
                UpdatedDate = productPackage.UpdatedDate,
                UpdatedBy = productPackage.UpdatedBy,
                UpdatedByName = productPackage.UpdatedByName,
                CreatedByName = productPackage.CreatedByName,
            };
            return result;
        }

        public async Task<PagingResponse<ProductPackageDto>> Handle(ProductPackagePagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductPackageDto>();
            var filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId))
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            var count = await _productPackageRepository.FilterCount( filter);
            var productPackages = await _productPackageRepository.Filter(filter, request.PageSize, request.PageIndex);
            var data = productPackages.Select(productPackage => new ProductPackageDto()
            {
                Id = productPackage.Id,
                ProductId = productPackage.ProductId,
                Name = productPackage.Name,
                Weight = productPackage.Weight,
                Length = productPackage.Length,
                Width = productPackage.Width,
                Height = productPackage.Height,
                CreatedDate = productPackage.CreatedDate,
                CreatedBy = productPackage.CreatedBy,
                UpdatedDate = productPackage.UpdatedDate,
                UpdatedBy = productPackage.UpdatedBy,
                UpdatedByName = productPackage.UpdatedByName,
                CreatedByName = productPackage.CreatedByName,
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductPackageDto>> Handle(ProductPackageQueryAll request, CancellationToken cancellationToken)
        {
            var productPackages = await _productPackageRepository.GetAll();
            var result = productPackages.Select(productPackage => new ProductPackageDto()
            {
                Id = productPackage.Id,
                ProductId = productPackage.ProductId,
                Name = productPackage.Name,
                Weight = productPackage.Weight,
                Length = productPackage.Length,
                Width = productPackage.Width,
                Height = productPackage.Height,
                CreatedDate = productPackage.CreatedDate,
                CreatedBy = productPackage.CreatedBy,
                UpdatedDate = productPackage.UpdatedDate,
                UpdatedBy = productPackage.UpdatedBy,
                UpdatedByName = productPackage.UpdatedByName,
                CreatedByName = productPackage.CreatedByName,
            });
            return result;
        }
        public async Task<List<ProductPackageDto>> Handle(ProductPackageQueryByProducts request, CancellationToken cancellationToken)
        {
            var items = await _productPackageRepository.GetByProducts(request.ProductList);
            var result = items.Select(item =>
            {
                return new ProductPackageDto()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Weight = item.Weight,
                    Length = item.Length,
                    Width = item.Width,
                    Height = item.Height
                };
            }
            ).ToList();
            return result;
        }
    }
}
