using Consul;
using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class TierPriceQueryAll : IQuery<IEnumerable<TierPriceDto>>
    {
        public TierPriceQueryAll()
        {
        }
    }

    public class TierPriceQueryById : IQuery<TierPriceDto>
    {
        public TierPriceQueryById()
        {
        }

        public TierPriceQueryById(Guid tierPriceId)
        {
            TierPriceId = tierPriceId;
        }

        public Guid TierPriceId { get; set; }
    }
    public class TierPriceQueryCheckExist : IQuery<bool>
    {

        public TierPriceQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class TierPricePagingQuery : ListQuery, IQuery<PagingResponse<TierPriceDto>>
    {
        public TierPricePagingQuery(TierPriceQueryParams tierPriceQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            QueryParams = tierPriceQueryParams;
        }

        public TierPricePagingQuery(TierPriceQueryParams tierPriceQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            QueryParams = tierPriceQueryParams;
        }

        public TierPriceQueryParams QueryParams { get; set; }
    }

    public class TierPriceQueryHandler : 
                                             IQueryHandler<TierPriceQueryAll, IEnumerable<TierPriceDto>>, 
                                             IQueryHandler<TierPriceQueryCheckExist, bool>,
                                             IQueryHandler<TierPriceQueryById, TierPriceDto>, 
                                             IQueryHandler<TierPricePagingQuery, PagingResponse<TierPriceDto>>
    {
        private readonly ITierPriceRepository _tierPriceRepository;
        private readonly IStoreRepository _storeRepository;
       public TierPriceQueryHandler(ITierPriceRepository tierPriceRespository, IStoreRepository storeRepository)
        {
            _tierPriceRepository = tierPriceRespository;
            _storeRepository = storeRepository;
        }
        public async Task<bool> Handle(TierPriceQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _tierPriceRepository.CheckExistById(request.Id);
        }
        public async Task<TierPriceDto> Handle(TierPriceQueryById request, CancellationToken cancellationToken)
        {
            var tierPrice = await _tierPriceRepository.GetById(request.TierPriceId);
            var result = new TierPriceDto()
            {
                Id = tierPrice.Id,
                ProductId = tierPrice.ProductId,
                CalculationMethod = tierPrice.CalculationMethod,
                CreatedBy = tierPrice.CreatedBy,
                CreatedDate = tierPrice.CreatedDate,
                EndDate = tierPrice.EndDate,
                Price = tierPrice.Price,
                Quantity = tierPrice.Quantity,
                StartDate = tierPrice.StartDate,
                StoreId = tierPrice.StoreId,
                UpdatedBy = tierPrice.UpdatedBy,
                UpdatedDate = tierPrice.UpdatedDate
            };
            return result;
        }

        public async Task<PagingResponse<TierPriceDto>> Handle(TierPricePagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<TierPriceDto>();
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
            if (!String.IsNullOrEmpty(request.QueryParams.StoreId))
            {
                filter.Add("storeId", request.QueryParams.StoreId);
            }
            var count = await _tierPriceRepository.FilterCount(filter);
            var tierPrices = await _tierPriceRepository.Filter(filter, request.PageSize, request.PageIndex);
            var stores = await _storeRepository.GetAll();
            var data = tierPrices.Select(tierPrice => new TierPriceDto()
            {
                Id = tierPrice.Id,
                ProductId= tierPrice.ProductId,
                CalculationMethod= tierPrice.CalculationMethod,
                CreatedBy = tierPrice.CreatedBy,
                CreatedDate = tierPrice.CreatedDate,
                EndDate= tierPrice.EndDate,
                Price= tierPrice.Price,
                Quantity= tierPrice.Quantity,
                StartDate= tierPrice.StartDate,
                StoreId= tierPrice.StoreId,
                StoreName = stores.ToList().Where(x => x.Id == tierPrice.StoreId).FirstOrDefault()?.Name,
                UpdatedBy= tierPrice.UpdatedBy,
                UpdatedDate = tierPrice.UpdatedDate
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }

        public async Task<IEnumerable<TierPriceDto>> Handle(TierPriceQueryAll request, CancellationToken cancellationToken)
        {
            var tierPrices = await _tierPriceRepository.GetAll();
            var result = tierPrices.Select(tierPrice => new TierPriceDto()
            {
                Id = tierPrice.Id,
                ProductId = tierPrice.ProductId,
                CalculationMethod = tierPrice.CalculationMethod,
                CreatedBy = tierPrice.CreatedBy,
                CreatedDate = tierPrice.CreatedDate,
                EndDate = tierPrice.EndDate,
                Price = tierPrice.Price,
                Quantity = tierPrice.Quantity,
                StartDate = tierPrice.StartDate,
                StoreId = tierPrice.StoreId,
                UpdatedBy = tierPrice.UpdatedBy,
                UpdatedDate = tierPrice.UpdatedDate
            });
            return result;
        }

     
    }
}
