using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class WarehouseQueryAll : IQuery<IEnumerable<WarehouseDto>>
    {
        public WarehouseQueryAll()
        {
        }
    }

    public class WarehouseQueryListBox : IQuery<IEnumerable<WarehouseListBoxDto>>
    {
        public WarehouseQueryListBox(string? keyword, int? status)
        {
            Keyword = keyword;
            Status = status;
        }
        public string? Keyword { get; set; }
        public int? Status { get; set; }
    }
    public class WarehouseQueryCheckExist : IQuery<bool>
    {

        public WarehouseQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class WarehouseQueryById : IQuery<WarehouseDto>
    {
        public WarehouseQueryById()
        {
        }

        public WarehouseQueryById(Guid warehouseId)
        {
            WarehouseId = warehouseId;
        }

        public Guid WarehouseId { get; set; }
    }
    public class WarehousePagingQuery : FopQuery, IQuery<PagedResult<List<WarehouseDto>>>
    {
        public WarehousePagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
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

    public class WarehouseQueryHandler : IQueryHandler<WarehouseQueryListBox, IEnumerable<WarehouseListBoxDto>>,
                                             IQueryHandler<WarehouseQueryAll, IEnumerable<WarehouseDto>>,
                                             IQueryHandler<WarehouseQueryCheckExist, bool>,
                                             IQueryHandler<WarehouseQueryById, WarehouseDto>,
                                             IQueryHandler<WarehousePagingQuery, PagedResult<List<WarehouseDto>>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        public WarehouseQueryHandler(IWarehouseRepository warehouseRespository)
        {
            _warehouseRepository = warehouseRespository;
        }
        public async Task<bool> Handle(WarehouseQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _warehouseRepository.CheckExistById(request.Id);
        }

        public async Task<WarehouseDto> Handle(WarehouseQueryById request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetById(request.WarehouseId);
            var result = new WarehouseDto()
            {
                Id = warehouse.Id,
                Code = warehouse.Code,
                Name = warehouse.Name,
                Address = warehouse.Address,
                Api = warehouse.Api,
                Company = warehouse.Company,
                Country = warehouse.Country,
                District = warehouse.District,
                Latitude = warehouse.Latitude,
                Longitude = warehouse.Longitude,
                PhoneNumber = warehouse.PhoneNumber,
                PostalCode = warehouse.PostalCode,
                Province = warehouse.Province,
                Token = warehouse.Token,
                Ward = warehouse.Ward,
                DisplayOrder = warehouse.DisplayOrder,
                CreatedBy = warehouse.CreatedBy,
                CreatedDate = warehouse.CreatedDate,
                CreatedByName = warehouse.CreatedByName,
                UpdatedBy = warehouse.UpdatedBy,
                UpdatedDate = warehouse.UpdatedDate,
                UpdatedByName = warehouse.UpdatedByName,
                Status = warehouse.Status,
            };
            return result;
        }

        public async Task<PagedResult<List<WarehouseDto>>> Handle(WarehousePagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<WarehouseDto>>();
            var fopRequest = FopExpressionBuilder<Warehouse>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (warehouses, count) = await _warehouseRepository.Filter(request.Keyword, fopRequest);
            var data = warehouses.Select(warehouse => new WarehouseDto()
            {
                Id = warehouse.Id,
                Code = warehouse.Code,
                Name = warehouse.Name,
                Address = warehouse.Address,
                Api = warehouse.Api,
                Company = warehouse.Company,
                Country = warehouse.Country,
                District = warehouse.District,
                Latitude = warehouse.Latitude,
                Longitude = warehouse.Longitude,
                PhoneNumber = warehouse.PhoneNumber,
                PostalCode = warehouse.PostalCode,
                Province = warehouse.Province,
                Token = warehouse.Token,
                Ward = warehouse.Ward,
                DisplayOrder = warehouse.DisplayOrder,
                CreatedBy = warehouse.CreatedBy,
                CreatedDate = warehouse.CreatedDate,
                CreatedByName = warehouse.CreatedByName,
                UpdatedBy = warehouse.UpdatedBy,
                UpdatedDate = warehouse.UpdatedDate,
                UpdatedByName = warehouse.UpdatedByName,
                Status = warehouse.Status,
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<WarehouseDto>> Handle(WarehouseQueryAll request, CancellationToken cancellationToken)
        {
            var warehouses = await _warehouseRepository.GetAll();
            var result = warehouses.Select(warehouse => new WarehouseDto()
            {
                Id = warehouse.Id,
                Code = warehouse.Code,
                Name = warehouse.Name,
                Address = warehouse.Address,
                Api = warehouse.Api,
                Company = warehouse.Company,
                Country = warehouse.Country,
                District = warehouse.District,
                Latitude = warehouse.Latitude,
                Longitude = warehouse.Longitude,
                PhoneNumber = warehouse.PhoneNumber,
                PostalCode = warehouse.PostalCode,
                Province = warehouse.Province,
                Token = warehouse.Token,
                Ward = warehouse.Ward,
                DisplayOrder = warehouse.DisplayOrder,
                CreatedBy = warehouse.CreatedBy,
                CreatedDate = warehouse.CreatedDate,
                UpdatedBy = warehouse.UpdatedBy,
                UpdatedDate = warehouse.UpdatedDate,
                Status = warehouse.Status
            });
            return result;
        }

        public async Task<IEnumerable<WarehouseListBoxDto>> Handle(WarehouseQueryListBox request, CancellationToken cancellationToken)
        {

            var warehouses = await _warehouseRepository.GetListListBox(request.Keyword, request.Status);
            var result = warehouses.Select(x => new WarehouseListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name,
                Address = x.Address,
                Country = x.Country,
                Province = x.Province,
                Ward = x.Ward,
                District = x.District
            });
            return result;
        }
    }
}
