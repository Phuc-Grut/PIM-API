using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Data;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ManufacturerQueryAll : IQuery<IEnumerable<ManufacturerDto>>
    {
        public ManufacturerQueryAll()
        {
        }
    }
    public class ManufacturerQueryCheckExist : IQuery<bool>
    {

        public ManufacturerQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ManufacturerQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ManufacturerQueryListBox(int? status, string? keyword)
        {
            Keyword = keyword;
            Status = status;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
    }
    public class ManufacturerQueryById : IQuery<ManufacturerDto>
    {
        public ManufacturerQueryById()
        {
        }

        public ManufacturerQueryById(Guid manufacturerId)
        {
            ManufacturerId = manufacturerId;
        }

        public Guid ManufacturerId { get; set; }
    }
    public class ManufacturerPagingQuery : FopQuery, IQuery<PagedResult<List<ManufacturerDto>>>
    {
        public ManufacturerPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
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

    public class ManufacturerQueryHandler : IQueryHandler<ManufacturerQueryListBox, IEnumerable<ListBoxDto>>,
                                            IQueryHandler<ManufacturerQueryAll, IEnumerable<ManufacturerDto>>,
                                            IQueryHandler<ManufacturerQueryCheckExist, bool>,
                                            IQueryHandler<ManufacturerQueryById, ManufacturerDto>,
                                            IQueryHandler<ManufacturerPagingQuery, PagedResult<List<ManufacturerDto>>>
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        public ManufacturerQueryHandler(IManufacturerRepository manufacturerRespository)
        {
            _manufacturerRepository = manufacturerRespository;
        }
        public async Task<bool> Handle(ManufacturerQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _manufacturerRepository.CheckExistById(request.Id);
        }

        public async Task<ManufacturerDto> Handle(ManufacturerQueryById request, CancellationToken cancellationToken)
        {
            var manufacturer = await _manufacturerRepository.GetById(request.ManufacturerId);
            var result = new ManufacturerDto()
            {
                Id = manufacturer.Id,
                Code = manufacturer.Code,
                Name = manufacturer.Name,
                Description= manufacturer.Description,
                DisplayOrder = manufacturer.DisplayOrder,
                Status = manufacturer.Status,
                CreatedBy = manufacturer.CreatedBy,
                CreatedDate = manufacturer.CreatedDate,
                UpdatedBy = manufacturer.UpdatedBy,
                UpdatedDate = manufacturer.UpdatedDate
            };
            return result;
        }

        public async Task<PagedResult<List<ManufacturerDto>>> Handle(ManufacturerPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<ManufacturerDto>>();
            var fopRequest = FopExpressionBuilder<Manufacturer>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var (manufacturers, count) = await _manufacturerRepository.Filter(request.Keyword, fopRequest);
            var data = manufacturers.Select(manufacturer => new ManufacturerDto()
            {
                Id = manufacturer.Id,
                Code = manufacturer.Code,
                Name = manufacturer.Name,
                Description = manufacturer.Description,
                DisplayOrder = manufacturer.DisplayOrder,
                Status = manufacturer.Status,
                CreatedBy = manufacturer.CreatedBy,
                CreatedDate = manufacturer.CreatedDate,
                CreatedByName = manufacturer.CreatedByName,
                UpdatedBy = manufacturer.UpdatedBy,
                UpdatedDate = manufacturer.UpdatedDate,
                UpdatedByName = manufacturer.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ManufacturerDto>> Handle(ManufacturerQueryAll request, CancellationToken cancellationToken)
        {
            var manufacturers = await _manufacturerRepository.GetAll();
            var result = manufacturers.Select(manufacturer => new ManufacturerDto()
            {
                Id = manufacturer.Id,
                Code = manufacturer.Code,
                Name = manufacturer.Name,
                Description = manufacturer.Description,
                DisplayOrder = manufacturer.DisplayOrder,
                Status = manufacturer.Status,
                CreatedBy = manufacturer.CreatedBy,
                CreatedDate = manufacturer.CreatedDate,
                UpdatedBy = manufacturer.UpdatedBy,
                UpdatedDate = manufacturer.UpdatedDate
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ManufacturerQueryListBox request, CancellationToken cancellationToken)
        {

            var manufacturers = await _manufacturerRepository.GetListListBox(request.Status, request.Keyword);
            var result = manufacturers.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
