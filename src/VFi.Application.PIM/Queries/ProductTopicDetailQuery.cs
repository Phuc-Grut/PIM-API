using Consul.Filtering;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Queries;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace VFi.Application.PIM.Queries
{

    public class ProductTopicDetailQueryAll : IQuery<IEnumerable<ProductTopicDetailDto>>
    {
        public ProductTopicDetailQueryAll()
        {
        }
    }

    public class ProductTopicDetailQueryListBox : IQuery<IEnumerable<ProductTopicDetailListBoxDto>>
    {
        public ProductTopicDetailQueryListBox(string? keyword)
        {
            Keyword = keyword;
        }
        public string? Keyword { get; set; }
    }
    public class ProductTopicDetailQueryCheckExist : IQuery<bool>
    {

        public ProductTopicDetailQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductTopicDetailQueryById : IQuery<ProductTopicDetailDto>
    {
        public ProductTopicDetailQueryById()
        {
        }

        public ProductTopicDetailQueryById(Guid itemId)
        {
            ProductTopicDetailId = itemId;
        }

        public Guid ProductTopicDetailId { get; set; }
    }
    public class ProductTopicDetailPagingQuery : ListQuery, IQuery<PagingResponse<ProductTopicDetailDto>>
    {
        public ProductTopicDetailPagingQuery():base()
        {
        }
        public ProductTopicDetailPagingQuery(string? keyword, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
        }

        public ProductTopicDetailPagingQuery(string? keyword, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            Keyword = keyword;
        }

        public string? Keyword { get; set; }
        public Guid? ProductTopicId { get; set; }
        public int? Status { get; set; }
        public string? Channel { get; set; }
        public Dictionary<string, object> Filter {
            get
            {
                var filter =  new Dictionary<string, object>();
                if (this.Status.HasValue)
                {
                    filter.Add("status", this.Status.Value); 
                }
                if (!string.IsNullOrEmpty(this.Channel))
                {
                    filter.Add("channel", this.Channel);
                }
                return filter;
            }
        }
    }
    public class ProductTopicDetailPagingByPageQuery : ListQuery, IQuery<PagingResponse<ProductTopicDetailDto>>
    {
        public ProductTopicDetailPagingByPageQuery() : base()
        { 
        }
        public ProductTopicDetailPagingByPageQuery(string? keyword,string? page, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        { 
            Keyword = keyword;
            Page = page;
        }

        public ProductTopicDetailPagingByPageQuery(string? keyword, string? page, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        { 
            Keyword = keyword;
            Page = page;
        }

        public string? Keyword { get; set; }
        public string? Page { get; set; }
        public int? Status { get; set; }
        public string? Channel { get; set; }
        public Dictionary<string, object> Filter
        {
            get
            {
                var filter = new Dictionary<string, object>();
                if (this.Status.HasValue)
                {
                    filter.Add("status", this.Status.Value);
                }
                if (!string.IsNullOrEmpty(this.Channel))
                {
                    filter.Add("channel", this.Channel);
                }
                return filter;
            }
        }
    }

    public class ProductTopicDetailLoopQuery : IQuery<IEnumerable<ProductTopicDetailDto>>
    {
        public ProductTopicDetailLoopQuery()
        {
            
        }
        public string? Channel { get; set; }
        public string? TopicPage { get; set; }
        public string? Topic { get; set; }
        public string? Keyword { get; set; }
        public int Top { get; set; }
    }
    public class ProductTopicDetailQueryHandler : IQueryHandler<ProductTopicDetailQueryListBox, IEnumerable<ProductTopicDetailListBoxDto>>,
                                             IQueryHandler<ProductTopicDetailQueryAll, IEnumerable<ProductTopicDetailDto>>,
                                             IQueryHandler<ProductTopicDetailQueryCheckExist, bool>,
                                             IQueryHandler<ProductTopicDetailQueryById, ProductTopicDetailDto>,
                                             IQueryHandler<ProductTopicDetailPagingByPageQuery, PagingResponse<ProductTopicDetailDto>>,
                                             IQueryHandler<ProductTopicDetailPagingQuery, PagingResponse<ProductTopicDetailDto>>,
                                             IQueryHandler<ProductTopicDetailLoopQuery, IEnumerable<ProductTopicDetailDto>>
    {
        private readonly IProductTopicDetailRepository _repository;
        private readonly IProductTopicPageMapRepository _repositoryMap;
        public ProductTopicDetailQueryHandler(IProductTopicDetailRepository itemRespository, IProductTopicPageMapRepository repositoryMap)
        {
            _repository = itemRespository; _repositoryMap = repositoryMap;
        }
        public async Task<bool> Handle(ProductTopicDetailQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _repository.CheckExistById(request.Id);
        }

        public async Task<ProductTopicDetailDto> Handle(ProductTopicDetailQueryById request, CancellationToken cancellationToken)
        {
            var item = await _repository.GetById(request.ProductTopicDetailId);
            var result = new ProductTopicDetailDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                ProductTopic = item.ProductTopic,
                Condition = item.Condition,
                Unit = item.Unit, 
                SourceLink = item.SourceLink,
                SourceCode = item.SourceCode,
                ShortDescription = item.ShortDescription,
                FullDescription = item.FullDescription,
                Origin = item.Origin,
                Brand = item.Brand,
                Manufacturer = item.Manufacturer,
                Image = item.Image,
                Images = item.Images,
                Price = item.Price,
                Currency = item.Currency,
                Status = item.Status,
                Tags = item.Tags,
                Exp = item.Exp,
                BidPrice = item.BidPrice,
                Tax = item.Tax,


                Channel = item.Channel,
                ShippingFee = item.ShippingFee,
                Bids = item.Bids,
                PublishDate = item.PublishDate,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                UpdatedByName = item.UpdatedByName
            };
            return result;
        }

        public async Task<PagingResponse<ProductTopicDetailDto>> Handle(ProductTopicDetailPagingQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductTopicDetailDto>();
            var count = await _repository.FilterCount(request.Keyword,request.Filter, request.ProductTopicId);
            var items = await _repository.Filter(request.Keyword, request.Filter, request.ProductTopicId, request.PageSize, request.PageIndex);
            var data = items.Select(item => new ProductTopicDetailDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                ProductTopic = item.ProductTopic,
                Condition = item.Condition,
                Unit = item.Unit,
                SourceLink = item.SourceLink,
                SourceCode = item.SourceCode,
                ShortDescription = item.ShortDescription,
               // FullDescription = item.FullDescription,
                Origin = item.Origin,
                Brand = item.Brand,
                Manufacturer = item.Manufacturer,
                Image = item.Image,
                Images = item.Images,
                Price = item.Price,
                Currency = item.Currency,
                Status = item.Status,
                Tags = item.Tags,
                Exp = item.Exp,
                BidPrice = item.BidPrice,
                Tax = item.Tax,
                Channel = item.Channel,
                ShippingFee = item.ShippingFee,
                Bids = item.Bids,
                PublishDate = item.PublishDate,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                UpdatedByName = item.UpdatedByName
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }
        public async Task<PagingResponse<ProductTopicDetailDto>> Handle(ProductTopicDetailPagingByPageQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductTopicDetailDto>();
            var listProductTopic = new List<Guid>();
            if(request.Page!=null && request.Page.Length > 0)
            {
                listProductTopic = (await _repositoryMap.GetListProductTopicIdByPage(request.Page)).ToList();
                if(!listProductTopic.Any()) listProductTopic.Add(Guid.Empty);
            } 
            var count = await _repository.FilterByPageCount(request.Keyword, request.Filter, listProductTopic);
            var items = await _repository.FilterByPage(request.Keyword, request.Filter, listProductTopic, request.PageSize, request.PageIndex);
            var data = items.Select(item => new ProductTopicDetailDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                ProductTopic = item.ProductTopic,
                Condition = item.Condition,
                Unit = item.Unit,
                SourceLink = item.SourceLink,
                SourceCode = item.SourceCode,
                ShortDescription = item.ShortDescription,
                // FullDescription = item.FullDescription,
                Origin = item.Origin,
                Brand = item.Brand,
                Manufacturer = item.Manufacturer,
                Image = item.Image,
                Images = item.Images,
                Price = item.Price,
                Currency = item.Currency,
                Status = item.Status,
                Tags = item.Tags,
                Exp = item.Exp,
                BidPrice = item.BidPrice,
                Tax = item.Tax,
                Channel = item.Channel,
                ShippingFee = item.ShippingFee,
                Bids = item.Bids,
                PublishDate = item.PublishDate,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                UpdatedByName = item.UpdatedByName
            });
            response.Items = data;
            response.Total = count; response.Count = count;
            response.PageIndex = request.PageIndex;
            response.PageSize = request.PageSize;
            response.Successful();
            return response;
        }
        public async Task<IEnumerable<ProductTopicDetailDto>> Handle(ProductTopicDetailQueryAll request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetAll();
            var result = items.Select(item => new ProductTopicDetailDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                ProductTopic = item.ProductTopic,
                Condition = item.Condition,
                Unit = item.Unit,
                SourceLink = item.SourceLink,
                SourceCode = item.SourceCode,
                ShortDescription = item.ShortDescription,
                //FullDescription = item.FullDescription,
                Origin = item.Origin,
                Brand = item.Brand,
                Manufacturer = item.Manufacturer,
                Image = item.Image,
                Images = item.Images,
                Price = item.Price,
                Currency = item.Currency,
                Status = item.Status,
                Tags = item.Tags,
                Exp = item.Exp,
                BidPrice = item.BidPrice,
                Tax = item.Tax,
                Channel = item.Channel,
                ShippingFee = item.ShippingFee,
                Bids = item.Bids,
                PublishDate = item.PublishDate,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                UpdatedByName = item.UpdatedByName
            });
            return result;
        }

        public async Task<IEnumerable<ProductTopicDetailListBoxDto>> Handle(ProductTopicDetailQueryListBox request, CancellationToken cancellationToken)
        {

            var items = await _repository.GetListListBox(request.Keyword);
            var result = items.Select(x => new ProductTopicDetailListBoxDto()
            {
                Key = x.Code,
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }


        public async Task<IEnumerable<ProductTopicDetailDto>> Handle(ProductTopicDetailLoopQuery request, CancellationToken cancellationToken)
        {
            var response = new PagingResponse<ProductTopicDetailDto>();
            var filter = new Dictionary<string, object>(); 
            var listProductTopic = new List<Guid>();
            if (!string.IsNullOrEmpty(request.TopicPage))
            { 
                listProductTopic = (await _repositoryMap.GetListProductTopicIdByPage(request.TopicPage)).ToList();
                if (!listProductTopic.Any()) listProductTopic.Add(Guid.Empty); 
            }
            if (!string.IsNullOrEmpty(request.Channel))
            {
                filter.Add("channel", request.Channel);
            }
            if (!string.IsNullOrEmpty(request.Topic))
            {
                filter.Add("topic", request.Topic);
            }
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                filter.Add("keyword", request.Keyword);
            } 
            filter.Add("status", 1);
               
            var items = await _repository.FilterByPage(request.Keyword, filter, listProductTopic, request.Top, 1);
            var data = items.Select(item => new ProductTopicDetailDto()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                ProductTopic = item.ProductTopic,
                Condition = item.Condition,
                Unit = item.Unit,
                SourceLink = item.SourceLink,
                SourceCode = item.SourceCode,
                ShortDescription = item.ShortDescription,
                // FullDescription = item.FullDescription,
                Origin = item.Origin,
                Brand = item.Brand,
                Manufacturer = item.Manufacturer,
                Image = item.Image,
                Images = item.Images,
                Price = item.Price,
                Currency = item.Currency,
                Status = item.Status,
                Tags = item.Tags,
                Exp = item.Exp,
                BidPrice = item.BidPrice,
                Tax = item.Tax,
                Channel = item.Channel,
                ShippingFee = item.ShippingFee,
                Bids = item.Bids,
                PublishDate = item.PublishDate,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                CreatedByName = item.CreatedByName,
                UpdatedBy = item.UpdatedBy,
                UpdatedDate = item.UpdatedDate,
                UpdatedByName = item.UpdatedByName
            }); 
            return data;
        }

    }
}
