using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductInventoryQueryAll : IQuery<IEnumerable<ProductInventoryDto>>
    {
        public ProductInventoryQueryAll()
        {
        }
    }

    public class ProductInventoryQueryById : IQuery<ProductInventoryDto>
    {
        public ProductInventoryQueryById()
        {
        }

        public ProductInventoryQueryById(Guid productInventoryId)
        {
            ProductInventoryId = productInventoryId;
        }

        public Guid ProductInventoryId { get; set; }
    }
    public class ProductInventoryQueryCheckExist : IQuery<bool>
    {

        public ProductInventoryQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductInventoryPagingQuery : ListQuery, IQuery<PagingResponse<ProductInventoryDto>>
    {
        public ProductInventoryPagingQuery(ProductInventoryQueryParams productInventoryQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productInventoryQueryParams;
        }

        public ProductInventoryPagingQuery( ProductInventoryQueryParams productInventoryQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productInventoryQueryParams;
        }

        public ProductInventoryQueryParams QueryParams { get; set; }
    }

    public class ProductInventoryQueryHandler :
                                             IQueryHandler<ProductInventoryQueryAll, IEnumerable<ProductInventoryDto>>,
                                             IQueryHandler<ProductInventoryQueryCheckExist, bool>,
                                             IQueryHandler<ProductInventoryQueryById, ProductInventoryDto>,
                                             IQueryHandler<ProductInventoryPagingQuery, PagingResponse<ProductInventoryDto>>
    {
        private readonly IProductInventoryRepository _productInventoryRepository;
        public ProductInventoryQueryHandler(IProductInventoryRepository productInventoryRespository)
        {
            _productInventoryRepository = productInventoryRespository;
        }
        public async Task<bool> Handle(ProductInventoryQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productInventoryRepository.CheckExistById(request.Id);
        }
        public async Task<ProductInventoryDto> Handle(ProductInventoryQueryById request, CancellationToken cancellationToken)
        {
            var productInventory = await _productInventoryRepository.GetById(request.ProductInventoryId);
            var result = new ProductInventoryDto()
            {
                Id = productInventory.Id,
                ProductId = productInventory.ProductId,
                WarehouseId = productInventory.WarehouseId,
                CreatedBy = productInventory.CreatedBy,
                CreatedDate = productInventory.CreatedDate,
                PlannedQuantity = productInventory.PlannedQuantity,
                ReservedQuantity = productInventory.ReservedQuantity,
                UpdatedBy = productInventory.UpdatedBy,
                UpdatedDate = productInventory.UpdatedDate,
                StockQuantity = productInventory.StockQuantity
            };
            return result;
        }

        public async Task<PagingResponse<ProductInventoryDto>> Handle(ProductInventoryPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductInventoryDto>();
            var filter = new Dictionary<string, object>();
            if (!String.IsNullOrEmpty(request.QueryParams.WarehouseId))
            {
                filter.Add("warehouseId", request.QueryParams.WarehouseId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId))
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            var count = await _productInventoryRepository.FilterCount( filter);
            var productInventorys = await _productInventoryRepository.Filter(filter, request.PageSize, request.PageIndex);
            var data = productInventorys.Select(productInventory => new ProductInventoryDto()
            {
                Id = productInventory.Id,
                ProductId = productInventory.ProductId,
                WarehouseId = productInventory.WarehouseId,
                CreatedBy = productInventory.CreatedBy,
                CreatedDate = productInventory.CreatedDate,
                PlannedQuantity = productInventory.PlannedQuantity,
                ReservedQuantity = productInventory.ReservedQuantity,
                UpdatedBy = productInventory.UpdatedBy,
                UpdatedDate = productInventory.UpdatedDate,
                StockQuantity = productInventory.StockQuantity
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductInventoryDto>> Handle(ProductInventoryQueryAll request, CancellationToken cancellationToken)
        {
            var productInventorys = await _productInventoryRepository.GetAll();
            var result = productInventorys.Select(productInventory => new ProductInventoryDto()
            {
                Id = productInventory.Id,
                ProductId = productInventory.ProductId,
                WarehouseId = productInventory.WarehouseId,
                CreatedBy = productInventory.CreatedBy,
                CreatedDate = productInventory.CreatedDate,
                PlannedQuantity = productInventory.PlannedQuantity,
                ReservedQuantity = productInventory.ReservedQuantity,
                UpdatedBy = productInventory.UpdatedBy,
                UpdatedDate = productInventory.UpdatedDate,
                StockQuantity = productInventory.StockQuantity
            });
            return result;
        }
    }
}
