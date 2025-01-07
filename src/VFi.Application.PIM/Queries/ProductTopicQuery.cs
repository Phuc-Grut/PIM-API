using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;

namespace VFi.Application.PIM.Queries
{

    public class ProductTopicQueryAll : IQuery<IEnumerable<ProductTopicDto>>
    {
        public ProductTopicQueryAll()
        {
        }
    }

    public class ProductTopicQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductTopicQueryListBox(int? status, string? keyword, Guid? productTopicPageId)
        {
            Status = status;
            Keyword = keyword;
            ProductTopicPageId = productTopicPageId;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
        public Guid? ProductTopicPageId { get; set; }
    }
    public class ProductTopicQueryCheckExist : IQuery<bool>
    {

        public ProductTopicQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductTopicQueryById : IQuery<ProductTopicDto>
    {
        public ProductTopicQueryById()
        {
        }

        public ProductTopicQueryById(Guid itemId)
        {
            ProductTopicId = itemId;
        }

        public Guid ProductTopicId { get; set; }
    }
    public class ProductTopicQueryByCode : IQuery<ProductTopicDto>
    {
        public ProductTopicQueryByCode()
        {
        }

        public ProductTopicQueryByCode(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
    public class ProductTopicQueryBySlug: IQuery<ProductTopicDto>
    {
        public ProductTopicQueryBySlug()
        {
        }

        public ProductTopicQueryBySlug(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; set; }
    }
    public class ProductTopicPagingQuery : FopQuery, IQuery<PagedResult<List<ProductTopicDto>>>
    {
        public ProductTopicPagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize, Guid? productTopicPageId)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
            ProductTopicPageId = productTopicPageId;
        }

        public string? Keyword { get; set; }
        public Guid? ProductTopicPageId { get; set; }
    }

    public class ProductTopicQueryHandler : IQueryHandler<ProductTopicQueryListBox, IEnumerable<ListBoxDto>>, 
                                             IQueryHandler<ProductTopicQueryAll, IEnumerable<ProductTopicDto>>, 
                                             IQueryHandler<ProductTopicQueryCheckExist, bool>,
                                             IQueryHandler<ProductTopicQueryById, ProductTopicDto>,
                                             IQueryHandler<ProductTopicQueryByCode, ProductTopicDto>,
        IQueryHandler<ProductTopicQueryBySlug, ProductTopicDto>,
                                             IQueryHandler<ProductTopicPagingQuery, PagedResult<List<ProductTopicDto>>>
    {
        private readonly IProductTopicRepository _itemRepository;
        public ProductTopicQueryHandler(IProductTopicRepository itemRespository)
        {
            _itemRepository = itemRespository;
        }
        public async Task<bool> Handle(ProductTopicQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _itemRepository.CheckExistById(request.Id);
            
        }

        public async Task<ProductTopicDto> Handle(ProductTopicQueryById request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetById(request.ProductTopicId);
            var result = new ProductTopicDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
                Slug = item.Slug,
                Keywords = item.Keywords,
                Image = item.Image,
                Icon= item.Icon,
                Icon2= item.Icon2, 
                Tags= item.Tags, 
                DisplayOrder = item.DisplayOrder,
                Status = item.Status,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName
            };
            return result;
        }
        public async Task<ProductTopicDto> Handle(ProductTopicQueryByCode request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetByCode(request.Code);
            var result = new ProductTopicDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
                Slug = item.Slug,
                Keywords = item.Keywords,
                Image = item.Image,
                Icon = item.Icon,
                Icon2 = item.Icon2,
                Tags = item.Tags,
                DisplayOrder = item.DisplayOrder,
                Status = item.Status,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName
            };
            return result;
        }
        public async Task<ProductTopicDto> Handle(ProductTopicQueryBySlug request, CancellationToken cancellationToken)
        {
            var item = await _itemRepository.GetBySlug(request.Slug);
            var result = new ProductTopicDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
                Slug = item.Slug,
                Keywords = item.Keywords,
                Image = item.Image,
                Icon = item.Icon,
                Icon2 = item.Icon2,
                Tags = item.Tags,
                DisplayOrder = item.DisplayOrder,
                Status = item.Status,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName
            };
            return result;
        }
        public async Task<PagedResult<List<ProductTopicDto>>> Handle(ProductTopicPagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<ProductTopic>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<ProductTopicDto>>();
            var (datas, count) = await _itemRepository.Filter(request.Keyword, request.ProductTopicPageId, fopRequest);
            var data = datas.Select(item => new ProductTopicDto()
            {
                Id = item.Id,
                ProductTopicPageIds = item.ProductTopicPageMap.Where(x => x.ProductTopicId == item.Id).Select(x => x.ProductTopicPageId),
                ProductTopicPageCodes = item.ProductTopicPageMap.Where(x => x.ProductTopicId == item.Id).Select(x => x.ProductTopicPage),
                Code = item.Code,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
                Slug = item.Slug,
                Keywords = item.Keywords,
                Image = item.Image,
                Icon = item.Icon,
                Icon2 = item.Icon2,
                Tags = item.Tags,
                DisplayOrder = item.DisplayOrder,
                Status = item.Status,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName
            }).ToList();
            response.Items = data;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }

        public async Task<IEnumerable<ProductTopicDto>> Handle(ProductTopicQueryAll request, CancellationToken cancellationToken)
        {
            var items = await _itemRepository.GetAll();
            var result = items.Select(item => new ProductTopicDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
                Slug = item.Slug,
                Keywords = item.Keywords,
                Image = item.Image,
                Icon = item.Icon,
                Icon2 = item.Icon2,
                Tags = item.Tags,
                DisplayOrder = item.DisplayOrder,
                Status = item.Status,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedByName = item.UpdatedByName
            });
            return result;
        }

        public async Task<IEnumerable<ListBoxDto>> Handle(ProductTopicQueryListBox request, CancellationToken cancellationToken)
        {

            var items = await _itemRepository.GetListListBox(request.Status, request.Keyword, request.ProductTopicPageId);
            var result = items.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
    }
}
