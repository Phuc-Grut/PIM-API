using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductServiceAddQueryAll : IQuery<IEnumerable<ProductServiceAddDto>>
    {
        public ProductServiceAddQueryAll()
        {
        }
    }

    public class ProductServiceAddQueryById : IQuery<ProductServiceAddDto>
    {
        public ProductServiceAddQueryById()
        {
        }

        public ProductServiceAddQueryById(Guid productServiceAddId)
        {
            ProductServiceAddId = productServiceAddId;
        }

        public Guid ProductServiceAddId { get; set; }
    }
    public class ProductServiceAddQueryCheckExist : IQuery<bool>
    {

        public ProductServiceAddQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductServiceAddPagingQuery : ListQuery, IQuery<PagingResponse<ProductServiceAddDto>>
    {
        public ProductServiceAddPagingQuery(ProductServiceAddQueryParams productServiceAddQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = productServiceAddQueryParams;
        }

        public ProductServiceAddPagingQuery(ProductServiceAddQueryParams productServiceAddQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = productServiceAddQueryParams;
        }

        public ProductServiceAddQueryParams QueryParams { get; set; }
    }

    public class ProductServiceAddQueryHandler : 
                                             IQueryHandler<ProductServiceAddQueryAll, IEnumerable<ProductServiceAddDto>>, 
                                             IQueryHandler<ProductServiceAddQueryCheckExist, bool>,
                                             IQueryHandler<ProductServiceAddQueryById, ProductServiceAddDto>, 
                                             IQueryHandler<ProductServiceAddPagingQuery, PagingResponse<ProductServiceAddDto>>
    {
        private readonly IProductServiceAddRepository _productServiceAddRepository;
        private readonly IServiceAddRepository _serviceAddRepository;
        public ProductServiceAddQueryHandler(IProductServiceAddRepository productServiceAddRespository, IServiceAddRepository serviceAddRepository)
        {
            _productServiceAddRepository = productServiceAddRespository;
            _serviceAddRepository = serviceAddRepository;
        }
        public async Task<bool> Handle(ProductServiceAddQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productServiceAddRepository.CheckExistById(request.Id);
        }
        public async Task<ProductServiceAddDto> Handle(ProductServiceAddQueryById request, CancellationToken cancellationToken)
        {
            var productServiceAdd = await _productServiceAddRepository.GetById(request.ProductServiceAddId);
            var result = new ProductServiceAddDto()
            {
                Id = productServiceAdd.Id,
                ProductId = productServiceAdd.ProductId,
                ServiceAddId = productServiceAdd.ServiceAddId,
                PayRequired = productServiceAdd.PayRequired,
                Price = productServiceAdd.Price,
                MaxPrice = productServiceAdd.MaxPrice,
                CalculationMethod = productServiceAdd.CalculationMethod,
                PriceSyntax = productServiceAdd.PriceSyntax,
                MinPrice = productServiceAdd.MinPrice,
                Currency = productServiceAdd.Currency,
                Status = productServiceAdd.Status,
                CreatedBy = productServiceAdd.CreatedBy,
                CreatedDate = productServiceAdd.CreatedDate,
                UpdatedBy = productServiceAdd.UpdatedBy,
                UpdatedDate = productServiceAdd.UpdatedDate,
                UpdatedByName = productServiceAdd.UpdatedByName,
                CreatedByName = productServiceAdd.CreatedByName
            };
            return result;
        }

        public async Task<PagingResponse<ProductServiceAddDto>> Handle(ProductServiceAddPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductServiceAddDto>();
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.StartDate != null)
            {
                filter.Add("startDate", request.QueryParams.StartDate);
            }
             if (request.QueryParams.EndDate != null)
            {
                filter.Add("endDate", request.QueryParams.EndDate);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.ProductId))
            {
                filter.Add("productId", request.QueryParams.ProductId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.ServiceAddId))
            {
                filter.Add("serviceAddId", request.QueryParams.ServiceAddId);
            }
            var count = await _productServiceAddRepository.FilterCount(filter);
            var productServiceAdds = await _productServiceAddRepository.Filter(filter, request.PageSize, request.PageIndex);
            var serviceAdds = await _serviceAddRepository.GetAll();
            var data = productServiceAdds.Select(productServiceAdd => new ProductServiceAddDto()
            {
                Id = productServiceAdd.Id,
                ProductId = productServiceAdd.ProductId,
                ServiceAddId = productServiceAdd.ServiceAddId,
                ServiceAddName = serviceAdds.Where(x => x.Id == productServiceAdd.ServiceAddId).FirstOrDefault()?.Name,
                PayRequired = productServiceAdd.PayRequired,
                Price = productServiceAdd.Price,
                MaxPrice = productServiceAdd.MaxPrice,
                CalculationMethod = productServiceAdd.CalculationMethod,
                PriceSyntax = productServiceAdd.PriceSyntax,
                MinPrice = productServiceAdd.MinPrice,
                Currency = productServiceAdd.Currency,
                Status = productServiceAdd.Status,
                CreatedBy = productServiceAdd.CreatedBy,
                CreatedDate = productServiceAdd.CreatedDate,
                UpdatedBy = productServiceAdd.UpdatedBy,
                UpdatedDate = productServiceAdd.UpdatedDate,
                UpdatedByName = productServiceAdd.UpdatedByName,
                CreatedByName = productServiceAdd.CreatedByName
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<ProductServiceAddDto>> Handle(ProductServiceAddQueryAll request, CancellationToken cancellationToken)
        {
            var productServiceAdds = await _productServiceAddRepository.GetAll();
            var result = productServiceAdds.Select(productServiceAdd => new ProductServiceAddDto()
            {
                Id = productServiceAdd.Id,
                ProductId = productServiceAdd.ProductId,
                ServiceAddId = productServiceAdd.ServiceAddId,
                PayRequired = productServiceAdd.PayRequired,
                Price = productServiceAdd.Price,
                MaxPrice = productServiceAdd.MaxPrice,
                CalculationMethod = productServiceAdd.CalculationMethod,
                PriceSyntax = productServiceAdd.PriceSyntax,
                MinPrice = productServiceAdd.MinPrice,
                Currency = productServiceAdd.Currency,
                Status = productServiceAdd.Status,
                CreatedBy = productServiceAdd.CreatedBy,
                CreatedDate = productServiceAdd.CreatedDate,
                UpdatedBy = productServiceAdd.UpdatedBy,
                UpdatedDate = productServiceAdd.UpdatedDate,
                UpdatedByName = productServiceAdd.UpdatedByName,
                CreatedByName = productServiceAdd.CreatedByName
            });
            return result;
        }

     
    }
}
