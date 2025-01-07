//using Consul.Filtering;
//using Consul;
//using VFi.Application.PIM.DTOs;
//using VFi.Domain.PIM.Interfaces;
//using VFi.Domain.PIM.Models;
//using VFi.NetDevPack.Queries;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;

//namespace VFi.Application.PIM.Queries
//{

//    public class ProductAttributeOptionQueryAll : IQuery<IEnumerable<ProductAttributeOptionDto>>
//    {
//        public ProductAttributeOptionQueryAll()
//        {
//        }
//    }

//    public class ProductAttributeOptionQueryListBox : IQuery<IEnumerable<ListBoxDto>>
//    {
//        public ProductAttributeOptionQueryListBox(string? productAttributeId, string? keyword)
//        {
//            Keyword = keyword;
//            Filter = new Dictionary<string, object>();
//            if (!String.IsNullOrEmpty(productAttributeId))
//            {
//                Filter.Add("productAttributeId", productAttributeId);
//            }
//        }
//        public string? Keyword { get; set; }
//        public Dictionary<string, object> Filter
//        {
//            get; set;
//        }
//    }
//    public class ProductAttributeOptionQueryById : IQuery<ProductAttributeOptionDto>
//    {
//        public ProductAttributeOptionQueryById()
//        {
//        }

//        public ProductAttributeOptionQueryById(Guid productAttributeOptionId)
//        {
//            ProductAttributeOptionId = productAttributeOptionId;
//        }

//        public Guid ProductAttributeOptionId { get; set; }
//    }
//    public class ProductAttributeOptionQueryCheckExist : IQuery<bool>
//    {
//        public ProductAttributeOptionQueryCheckExist(Guid id)
//        {
//            Id = id;
//        }

//        public Guid Id { get; set; }
//    }
//    public class ProductAttributeOptionPagingQuery : ListQuery, IQuery<PagingResponse<ProductAttributeOptionDto>>
//    {
//        public ProductAttributeOptionPagingQuery(string? keyword, string? productAttributeId, int pageSize, int pageIndex) : base(pageSize, pageIndex)
//        {
//            Keyword = keyword;
//            Filter = new Dictionary<string, object>();
//            if (!String.IsNullOrEmpty(productAttributeId))
//            {
//                Filter.Add("productAttributeId", productAttributeId);
//            }
//        }

//        public ProductAttributeOptionPagingQuery(string? keyword, string? productAttributeId, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex)
//        {
//            Keyword = keyword;
//            Filter = new Dictionary<string, object>();
//            if (!String.IsNullOrEmpty(productAttributeId))
//            {
//                Filter.Add("productAttributeId", productAttributeId);
//            }
//        }
//        public string? Keyword { get; set; }
//        public Dictionary<string, object> Filter
//        {
//            get; set;
//        }
//    }

//    public class ProductAttributeOptionQueryHandler : IQueryHandler<ProductAttributeOptionQueryListBox, IEnumerable<ListBoxDto>>,
//                                            IQueryHandler<ProductAttributeOptionQueryAll, IEnumerable<ProductAttributeOptionDto>>,
//                                            IQueryHandler<ProductAttributeOptionQueryById, ProductAttributeOptionDto>,
//                                            IQueryHandler<ProductAttributeOptionQueryCheckExist, bool>,
//                                            IQueryHandler<ProductAttributeOptionPagingQuery, PagingResponse<ProductAttributeOptionDto>>
//    {
//        private readonly IProductAttributeOptionRepository _productAttributeOptionRepository;
//        public ProductAttributeOptionQueryHandler(IProductAttributeOptionRepository productAttributeOptionRespository)
//        {
//            _productAttributeOptionRepository = productAttributeOptionRespository;
//        }
//        public async Task<bool> Handle(ProductAttributeOptionQueryCheckExist request, CancellationToken cancellationToken)
//        {
//            return await _productAttributeOptionRepository.CheckExistById(request.Id);
//        }
//        public async Task<ProductAttributeOptionDto> Handle(ProductAttributeOptionQueryById request, CancellationToken cancellationToken)
//        {
//            var productAttributeOption = await _productAttributeOptionRepository.GetById(request.ProductAttributeOptionId);
//            var result = new ProductAttributeOptionDto()
//            {
//                Id = productAttributeOption.Id,
//                Name = productAttributeOption.Name,
//                Alias = productAttributeOption.Alias,
//                MediaFileId = productAttributeOption.MediaFileId,
//                Color = productAttributeOption.Color,
//                IsPreSelected = productAttributeOption.IsPreSelected,
//                LinkedProductId = productAttributeOption.LinkedProductId,
//                PriceAdjustment = productAttributeOption.PriceAdjustment,
//                ProductAttributeId = productAttributeOption.ProductAttributeId,
//                Quantity = productAttributeOption.Quantity,
//                ValueTypeId = productAttributeOption.ValueTypeId,
//                WeightAdjustment = productAttributeOption.WeightAdjustment,
//                DisplayOrder = productAttributeOption.DisplayOrder,
//                CreatedBy = productAttributeOption.CreatedBy,
//                CreatedDate = productAttributeOption.CreatedDate,
//                UpdatedBy = productAttributeOption.UpdatedBy,
//                UpdatedDate = productAttributeOption.UpdatedDate
//            };
//            return result;
//        }

//        public async Task<PagingResponse<ProductAttributeOptionDto>> Handle(ProductAttributeOptionPagingQuery request, CancellationToken cancellationToken)
//        {
//            var response = new PagingResponse<ProductAttributeOptionDto>();
//            var count = await _productAttributeOptionRepository.FilterCount(request.Keyword, request.Filter);
//            var productAttributeOptions = await _productAttributeOptionRepository.Filter(request.Keyword, request.Filter, request.PageSize, request.PageIndex);
//            var data = productAttributeOptions.Select(productAttributeOption => new ProductAttributeOptionDto()
//            {
//                Id = productAttributeOption.Id,
//                Name = productAttributeOption.Name,
//                Alias = productAttributeOption.Alias,
//                MediaFileId = productAttributeOption.MediaFileId,
//                Color = productAttributeOption.Color,
//                IsPreSelected = productAttributeOption.IsPreSelected,
//                LinkedProductId = productAttributeOption.LinkedProductId,
//                PriceAdjustment = productAttributeOption.PriceAdjustment,
//                ProductAttributeId = productAttributeOption.ProductAttributeId,
//                Quantity = productAttributeOption.Quantity,
//                ValueTypeId = productAttributeOption.ValueTypeId,
//                WeightAdjustment = productAttributeOption.WeightAdjustment,
//                DisplayOrder = productAttributeOption.DisplayOrder,
//                CreatedBy = productAttributeOption.CreatedBy,
//                CreatedDate = productAttributeOption.CreatedDate,
//                UpdatedBy = productAttributeOption.UpdatedBy,
//                UpdatedDate = productAttributeOption.UpdatedDate
//            });
//            response.Items = data;
//            response.Total = count; response.Count = count;
//            response.PageIndex = request.PageIndex;
//            response.PageSize = request.PageSize;
//            response.Successful();
//            return response;
//        }

//        public async Task<IEnumerable<ProductAttributeOptionDto>> Handle(ProductAttributeOptionQueryAll request, CancellationToken cancellationToken)
//        {
//            var productAttributeOptions = await _productAttributeOptionRepository.GetAll();
//            var result = productAttributeOptions.Select(productAttributeOption => new ProductAttributeOptionDto()
//            {
//                Id = productAttributeOption.Id,
//                Name = productAttributeOption.Name,
//                Alias = productAttributeOption.Alias,
//                MediaFileId = productAttributeOption.MediaFileId,
//                Color = productAttributeOption.Color,
//                IsPreSelected = productAttributeOption.IsPreSelected,
//                LinkedProductId = productAttributeOption.LinkedProductId,
//                PriceAdjustment = productAttributeOption.PriceAdjustment,
//                ProductAttributeId = productAttributeOption.ProductAttributeId,
//                Quantity = productAttributeOption.Quantity,
//                ValueTypeId = productAttributeOption.ValueTypeId,
//                WeightAdjustment = productAttributeOption.WeightAdjustment,
//                DisplayOrder = productAttributeOption.DisplayOrder,
//                CreatedBy = productAttributeOption.CreatedBy,
//                CreatedDate = productAttributeOption.CreatedDate,
//                UpdatedBy = productAttributeOption.UpdatedBy,
//                UpdatedDate = productAttributeOption.UpdatedDate
//            });
//            return result;
//        }

//        public async Task<IEnumerable<ListBoxDto>> Handle(ProductAttributeOptionQueryListBox request, CancellationToken cancellationToken)
//        {

//            var productAttributeOptions = await _productAttributeOptionRepository.GetListListBox(request.Filter, request.Keyword);
//            var result = productAttributeOptions.Select(x => new ListBoxDto()
//            {
//                Value = x.Id,
//                Label = x.Name
//            });
//            return result;
//        }
//    }
//}
