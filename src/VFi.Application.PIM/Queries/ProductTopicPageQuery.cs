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

    public class ProductTopicPageQueryAll : IQuery<IEnumerable<ProductTopicPageDto>>
    {
        public ProductTopicPageQueryAll()
        {
        }
    }
    public class ProductTopicPageQueryAllByStatus : IQuery<IEnumerable<ProductTopicPageDto>>
    {
        public int? Status { get; set; }
        public ProductTopicPageQueryAllByStatus()
        {
        }
    }
    public class ProductTopicPageQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductTopicPageQueryListBox(int? status, string? keyword)
        {
            Status = status;
            Keyword = keyword;
        }
        public int? Status { get; set; }
        public string? Keyword { get; set; }
    }
    public class ProductTopicPageQueryCheckExist : IQuery<bool>
    {

        public ProductTopicPageQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductTopicPageQueryById : IQuery<ProductTopicPageDto>
    {
        public ProductTopicPageQueryById()
        {
        }

        public ProductTopicPageQueryById(Guid itemId)
        {
            ProductTopicId = itemId;
        }

        public Guid ProductTopicId { get; set; }
    }
    public class ProductTopicPageQueryByCode : IQuery<ProductTopicPageDto>
    {
        public ProductTopicPageQueryByCode()
        {
        }

        public ProductTopicPageQueryByCode(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
    public class ProductTopicQueryByPage: IQuery<IEnumerable<ProductTopicDto>>
    {
        public ProductTopicQueryByPage()
        {
        }

        public ProductTopicQueryByPage(string code)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
    public class ProductTopicPageQueryBySlug : IQuery<ProductTopicPageDto>
    {
        public ProductTopicPageQueryBySlug()
        {
        }

        public ProductTopicPageQueryBySlug(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; set; }
    }
   
    public class ProductTopicPagePagingQuery : FopQuery, IQuery<PagedResult<List<ProductTopicPageDto>>>
    {
        public ProductTopicPagePagingQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
        {
            Keyword = keyword;
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public string? Keyword { get; set; }
    }
    public class ProductTopicQueryByPageSlug : IQuery<IEnumerable<ProductTopicDto>>
    {
        public ProductTopicQueryByPageSlug()
        {
        }

        public ProductTopicQueryByPageSlug(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; set; }
    }
    public class ProductTopicQueryByPageId : IQuery<IEnumerable<ProductTopicDto>>
    {
        public ProductTopicQueryByPageId()
        {
        }

        public ProductTopicQueryByPageId(Guid pageId)
        {
            PageId = pageId;
        }

        public Guid PageId { get; set; }
    }
 
    public class ProductTopicPageQueryHandler : IQueryHandler<ProductTopicPageQueryListBox, IEnumerable<ListBoxDto>>, 
                                             IQueryHandler<ProductTopicPageQueryAll, IEnumerable<ProductTopicPageDto>>,
          IQueryHandler<ProductTopicPageQueryAllByStatus, IEnumerable<ProductTopicPageDto>>,
                                             IQueryHandler<ProductTopicPageQueryCheckExist, bool>,
                                             IQueryHandler<ProductTopicPageQueryById, ProductTopicPageDto>,
                                             IQueryHandler<ProductTopicPageQueryByCode, ProductTopicPageDto>,
        IQueryHandler<ProductTopicPageQueryBySlug, ProductTopicPageDto>,
                                             IQueryHandler<ProductTopicQueryByPage, IEnumerable<ProductTopicDto>>,
                                            IQueryHandler<ProductTopicQueryByPageSlug, IEnumerable<ProductTopicDto>>,
        IQueryHandler<ProductTopicQueryByPageId, IEnumerable<ProductTopicDto>>,
                                             IQueryHandler<ProductTopicPagePagingQuery, PagedResult<List<ProductTopicPageDto>>>
    {
        private readonly IProductTopicPageRepository _repository;
        private readonly IProductTopicPageMapRepository _repositoryMap;
        public ProductTopicPageQueryHandler(IProductTopicPageRepository itemRespository, IProductTopicPageMapRepository repositoryMap)
        {
            _repository = itemRespository;
            _repositoryMap = repositoryMap;
        }
        public async Task<bool> Handle(ProductTopicPageQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _repository.CheckExistById(request.Id);
            
        }

        public async Task<ProductTopicPageDto> Handle(ProductTopicPageQueryById request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetById(request.ProductTopicId);
            var result = new ProductTopicPageDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Title = item.Title,
                Description = item.Description,
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
        public async Task<ProductTopicPageDto> Handle(ProductTopicPageQueryByCode request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetByCode(request.Code);
            var result = new ProductTopicPageDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Slug = item.Slug,
                Title = item.Title,
                Description = item.Description,
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
        public async Task<ProductTopicPageDto> Handle(ProductTopicPageQueryBySlug request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetBySlug(request.Slug);
            var result = new ProductTopicPageDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Title = item.Title,
                Slug = item.Slug,
                Description = item.Description,
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
        public async Task<PagedResult<List<ProductTopicPageDto>>> Handle(ProductTopicPagePagingQuery request, CancellationToken cancellationToken)
        {
            var fopRequest = FopExpressionBuilder<ProductTopicPage>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);
            var response = new PagedResult<List<ProductTopicPageDto>>();
            var (datas, count) = await _repository.Filter(request.Keyword, fopRequest);
            var data = datas.Select(item => new ProductTopicPageDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name, 
                Slug = item.Slug,
                Title = item.Title,
                Description = item.Description,
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

        public async Task<IEnumerable<ProductTopicPageDto>> Handle(ProductTopicPageQueryAll request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAll();
            var result = items.Select(item => new ProductTopicPageDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Slug = item.Slug,
                Title = item.Title,
                Description = item.Description,
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
        public async Task<IEnumerable<ProductTopicPageDto>> Handle(ProductTopicPageQueryAllByStatus request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAll(request.Status);
            var result = items.Select(item => new ProductTopicPageDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Slug = item.Slug,
                Title = item.Title,
                Description = item.Description,
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
        public async Task<IEnumerable<ListBoxDto>> Handle(ProductTopicPageQueryListBox request, CancellationToken cancellationToken)
        {

            var items = await _repository.GetListListBox(request.Status, request.Keyword);
            var result = items.Select(x => new ListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }
        public async Task<IEnumerable<ProductTopicDto>> Handle(ProductTopicQueryByPage request, CancellationToken cancellationToken)
        {
            var items = await _repositoryMap.GetProductTopic(request.Code);

            var result = items.Select(item => new ProductTopicDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Slug = item.Slug,
                Title = item.Title,
                Description = item.Description,
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
        public async Task<IEnumerable<ProductTopicDto>> Handle(ProductTopicQueryByPageSlug request, CancellationToken cancellationToken)
        {
            var items = await _repositoryMap.GetProductTopicBySlugPage(request.Slug);
            //var topicPage  = await _repository.GetBySlug(request.Slug);
            var result = items.Select(item => new ProductTopicDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Slug = item.Slug,
                Title = item.Title,
                Description = item.Description,
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
        public async Task<IEnumerable<ProductTopicDto>> Handle(ProductTopicQueryByPageId request, CancellationToken cancellationToken)
        {
            var items = await _repositoryMap.GetProductTopicByPageId(request.PageId);

            var result = items.Select(item => new ProductTopicDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Slug = item.Slug,
                Title = item.Title,
                Description = item.Description,
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
    }
}
