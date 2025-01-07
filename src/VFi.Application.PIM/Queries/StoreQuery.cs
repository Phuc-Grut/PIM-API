using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class StoreQueryAll : IQuery<IEnumerable<StoreDto>>
    {
        public StoreQueryAll()
        {
        }
    }

    public class StoreQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public StoreQueryListBox(string? keyword)
        {
            Keyword = keyword;
        }
        public string? Keyword { get; set; }
    }
    public class StoreQueryCheckExist : IQuery<bool>
    {

        public StoreQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class StoreQueryById : IQuery<StoreDto>
    {
        public StoreQueryById()
        {
        }

        public StoreQueryById(Guid storeId)
        {
            StoreId = storeId;
        }

        public Guid StoreId { get; set; }
    }
    public class StorePagingQuery : FopQuery, IQuery<PagedResult<List<StoreDto>>>
    {
        public StorePagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
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

    public class StoreQueryHandler : IQueryHandler<StoreQueryListBox, IEnumerable<ListBoxDto>>,
                                             IQueryHandler<StoreQueryAll, IEnumerable<StoreDto>>,
                                             IQueryHandler<StoreQueryCheckExist, bool>,
                                             IQueryHandler<StoreQueryById, StoreDto>,
                                             IQueryHandler<StorePagingQuery, PagedResult<List<StoreDto>>>
    {
        private readonly IStoreRepository _storeRepository;
        public StoreQueryHandler(IStoreRepository storeRespository)
        {
            _storeRepository = storeRespository;
        }
        public async Task<bool> Handle(StoreQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _storeRepository.CheckExistById(request.Id);
        }

        public async Task<StoreDto> Handle(StoreQueryById request, CancellationToken cancellationToken)
        {
            var store = await _storeRepository.GetById(request.StoreId);
            var result = new StoreDto()
            {
                Id = store.Id,
                Code = store.Code,
                Name = store.Name,
                Description = store.Description,
                Address = store.Address,
                Phone = store.Phone,
                DisplayOrder = store.DisplayOrder,
                CreatedBy = store.CreatedBy,
                CreatedDate = store.CreatedDate,
                CreatedByName = store.CreatedByName,
                UpdatedBy = store.UpdatedBy,
                UpdatedDate = store.UpdatedDate,
                UpdatedByName = store.UpdatedByName
            };
            return result;
        }

        public async Task<PagedResult<List<StoreDto>>> Handle(StorePagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<StoreDto>>();
            var fopRequest = FopExpressionBuilder<Store>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (stores, count) = await _storeRepository.Filter(request.Keyword, fopRequest);
            var data = stores.Select(store => new StoreDto()
            {
                Id = store.Id,
                Code = store.Code,
                Name = store.Name,
                Description = store.Description,
                Address = store.Address,
                Phone = store.Phone,
                DisplayOrder = store.DisplayOrder,
                CreatedBy = store.CreatedBy,
                CreatedDate = store.CreatedDate,
                CreatedByName = store.CreatedByName,
                UpdatedBy = store.UpdatedBy,
                UpdatedDate = store.UpdatedDate,
                UpdatedByName = store.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<StoreDto>> Handle(StoreQueryAll request, CancellationToken cancellationToken)
        {
            var stores = await _storeRepository.GetAll();
            var result = stores.Select(store => new StoreDto()
            {
                Id = store.Id,
                Code = store.Code,
                Name = store.Name,
                Description = store.Description,
                Address = store.Address,
                Phone = store.Phone,
                DisplayOrder = store.DisplayOrder,
                CreatedBy = store.CreatedBy,
                CreatedDate = store.CreatedDate,
                UpdatedBy = store.UpdatedBy,
                UpdatedDate = store.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(StoreQueryListBox request, CancellationToken cancellationToken)
        {

            var stores = await _storeRepository.GetListListBox(request.Keyword);
            var result = stores.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
