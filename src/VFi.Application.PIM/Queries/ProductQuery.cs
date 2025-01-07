using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Domain;
using VFi.NetDevPack.FopExpression;
using VFi.NetDevPack.Queries;
using VFi.NetDevPack.Utilities;

namespace VFi.Application.PIM.Queries
{

    public class ProductQueryAll : IQuery<IEnumerable<ProductDto>>
    {
        public ProductQueryAll()
        {
        }
    }
    public class ProductQueryListBox : IQuery<IEnumerable<ListBoxDto>>
    {
        public ProductQueryListBox(ProductQueryParams productQueryParams, string? keyword)
        {
            Keyword = keyword;
            QueryParams = productQueryParams;
        }
        public string? Keyword { get; set; }
        public ProductQueryParams QueryParams { get; set; }
    }
    public class ProductQueryById : IQuery<ProductFullDto>
    {
        public ProductQueryById()
        {
        }

        public ProductQueryById(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; set; }
    }
    public class ProductQueryByCategoryRootId : IQuery<string>
    {
        public ProductQueryByCategoryRootId(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
    public class ProductQueryByProductCode : IQuery<ProductFullDto>
    {
        public ProductQueryByProductCode()
        {
        }

        public ProductQueryByProductCode(string code)
        {
            Code = code;
        }

        public string? Code { get; set; }
    }
    public class ProductQueryByCode : IQuery<ProductDuplicateDto>
    {
        public ProductQueryByCode()
        {
        }

        public ProductQueryByCode(string code)
        {
            Code = code;
        }

        public string? Code { get; set; }
    }

    public class ProductInventoryQueryByCode : IQuery<List<ProductsInventoryDto>>
    {
        public ProductInventoryQueryByCode()
        {
        }

        public ProductInventoryQueryByCode(List<string> productList)
        {
            ProductList = productList;
        }

        public List<string> ProductList { get; set; }
    }
    public class ProductInventoryQueryByListId : IQuery<List<SP_GET_INVENTORY_BY_LISTID>>
    {
        public ProductInventoryQueryByListId()
        {
        }

        public ProductInventoryQueryByListId(string productList)
        {
            ProductList = productList;
        }

        public string ProductList { get; set; }
    }
    public class ProductStockQueryById : IQuery<SP_GET_INVENTORY_BY_ID>
    {
        public ProductStockQueryById(Guid id)
        {
            ProductId = id;
        }

        public Guid ProductId { get; set; }
    }
    public class ProductListWarehouseQueryById : IQuery<List<SP_GET_INVENTORY_DETAIL>>
    {
        public ProductListWarehouseQueryById(Guid id)
        {
            ProductId = id;
        }

        public Guid ProductId { get; set; }
    }
    public class ProductQueryPrice : IQuery<List<SP_GET_PRODUCT_PRICE_BY_LISTID>>
    {
        public ProductQueryPrice(string productList)
        {
            ProductList = productList;
        }

        public string ProductList { get; set; }
    }
    public class ProductQueryInventoryDetailByListId : IQuery<List<SP_GET_INVENTORY_DETAIL_BY_LISTID>>
    {
        public ProductQueryInventoryDetailByListId(string productList)
        {
            ProductList = productList;
        }

        public string ProductList { get; set; }
    }
    public class ProductQueryCheckExist : IQuery<bool>
    {

        public ProductQueryCheckExist(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; set; }
    }
    public class ProductPagingQuery : ListQuery, IQuery<PagingResponse<ProductDto>>
    {
        public ProductPagingQuery(string? keyword, ProductQueryParams productQueryParams, int pageSize, int pageIndex) : base(pageSize, pageIndex)
        {
            Keyword = keyword;
            QueryParams = productQueryParams;
        }

        public ProductPagingQuery(string? keyword, ProductQueryParams productQueryParams, int pageSize, int pageIndex, SortingInfo[] sortingInfos) : base(pageSize, pageIndex, sortingInfos)
        {
            Keyword = keyword;
            QueryParams = productQueryParams;
        }

        public string? Keyword { get; set; }
        public ProductQueryParams QueryParams { get; set; }
    }
    public class ProductPagingFilterQuery : FopQuery, IQuery<PagedResult<List<ProductDto>>>
    {
        public ProductPagingFilterQuery(string? filter, string? order, int pageNumber, int pageSize)
        {
            Filter = filter;
            Order = order;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public ProductPagingFilterQuery(string? keyword, string? filter, string? order, int pageNumber, int pageSize)
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

    public class ProductQueryByListCode : IQuery<IEnumerable<ProductDto>>
    {
        public ProductQueryByListCode(List<string> codes)
        {
            Codes = codes;
        }

        public List<string> Codes { get; set; }
    }

    public class ProductQueryHandler : IQueryHandler<ProductQueryListBox, IEnumerable<ListBoxDto>>,
                                             IQueryHandler<ProductQueryCheckExist, bool>,
                                             IQueryHandler<ProductQueryByCategoryRootId, string>,
                                             IQueryHandler<ProductQueryById, ProductFullDto>,
                                             IQueryHandler<ProductQueryByProductCode, ProductFullDto>,
                                             IQueryHandler<ProductInventoryQueryByCode, List<ProductsInventoryDto>>,
                                             IQueryHandler<ProductInventoryQueryByListId, List<SP_GET_INVENTORY_BY_LISTID>>,
                                             IQueryHandler<ProductStockQueryById, SP_GET_INVENTORY_BY_ID>,
                                             IQueryHandler<ProductListWarehouseQueryById, List<SP_GET_INVENTORY_DETAIL>>,
                                             IQueryHandler<ProductPagingFilterQuery, PagedResult<List<ProductDto>>>,
                                             IQueryHandler<ProductQueryPrice, List<SP_GET_PRODUCT_PRICE_BY_LISTID>>,
                                             IQueryHandler<ProductQueryInventoryDetailByListId, List<SP_GET_INVENTORY_DETAIL_BY_LISTID>>,
                                             IQueryHandler<ProductQueryByCode, ProductDuplicateDto>,
                                             IQueryHandler<ProductQueryByListCode, IEnumerable<ProductDto>>

    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IProductStoreMappingRepository _productStoreMappingRepository;
        private readonly IProductGroupCategoryMappingRepository _productGroupCategoryMappingRepository;
        private readonly IProductProductTagMappingRepository _productProductTagMappingRepository;
        private readonly IProductCategoryMappingRepository _productCategoryMappingRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITierPriceRepository _tierPriceRepository;
        private readonly IProductServiceAddRepository _productServiceAddRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IProductInventoryRepository _productInventoryRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IGroupUnitRepository _groupUnitRepository;
        private readonly IPIMContextProcedures _pimContextProcedures;
        public ProductQueryHandler(IProductRepository productRespository, IProductTagRepository productTagRepository,
            IProductStoreMappingRepository productStoreMappingRepository,
            IProductGroupCategoryMappingRepository productGroupCategoryMappingRepository,
            IProductCategoryMappingRepository productCategoryMappingRepository,
            IProductProductTagMappingRepository productProductTagMappingRepository,
            ICategoryRepository categoryRepository,
            IGroupUnitRepository groupUnitRepository,
            ITierPriceRepository tierPriceRepository,
            IProductServiceAddRepository productServiceAddRepository,
            IStoreRepository storeRepository,
            IUnitRepository unitRepository,
            IProductInventoryRepository productInventoryRepository,
            IWarehouseRepository warehouseRepository,
            IPIMContextProcedures pimContextProcedures
            )
        {
            _productRepository = productRespository;
            _productTagRepository = productTagRepository;
            _productStoreMappingRepository = productStoreMappingRepository;
            _productGroupCategoryMappingRepository = productGroupCategoryMappingRepository;
            _productCategoryMappingRepository = productCategoryMappingRepository;
            _productProductTagMappingRepository = productProductTagMappingRepository;
            _categoryRepository = categoryRepository;
            _tierPriceRepository = tierPriceRepository;
            _groupUnitRepository = groupUnitRepository;
            _productServiceAddRepository = productServiceAddRepository;
            _storeRepository = storeRepository;
            _unitRepository = unitRepository;
            _productInventoryRepository = productInventoryRepository;
            _warehouseRepository = warehouseRepository;
            _pimContextProcedures = pimContextProcedures;
        }
        public async Task<bool> Handle(ProductQueryCheckExist request, CancellationToken cancellationToken)
        {
            return await _productRepository.CheckExistById(request.Id);
        }
        public async Task<string> Handle(ProductQueryByCategoryRootId request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetByCategoryRootId(request.Id);
        }
        public async Task<ProductFullDto> Handle(ProductQueryById request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetById(request.ProductId);
            if (product.ParentId != null)
            {
                var result = new ProductFullDto()
                {
                    Id = product.Id,
                    Code = product.Code,
                    ProductType = product.ProductType,
                    ForBuy = product.ForBuy,
                    ForSale = product.ForSale,
                    ForProduction = product.ForProduction,
                    Condition = product.Condition,
                    UnitId = product.UnitId,
                    UnitCode = product.UnitCode,
                    UnitType = product.UnitType,
                    Name = product.Name,
                    SourceLink = product.SourceLink,
                    SourceCode = product.SourceCode,
                    ShortDescription = product.ShortDescription,
                    FullDescription = product.FullDescription.HtmlStandardized(),
                    LimitedToStores = product.LimitedToStores,
                    IdGroupCategories = product.IdGroupCategories,
                    GroupCategories = product.GroupCategories,
                    Categories = product.Categories,
                    Origin = product.Origin,
                    Brand = product.Brand,
                    Manufacturer = product.Manufacturer,
                    ManufacturerNumber = product.ManufacturerNumber,
                    Image = product.Image,
                    Gtin = product.Gtin,
                    ProductCost = product.ProductCost,
                    CurrencyCost = product.CurrencyCost,
                    Price = product.Price,
                    HasTierPrices = product.HasTierPrices,
                    Currency = product.Currency,
                    IsTaxExempt = product.IsTaxExempt,
                    TaxCategoryId = product.TaxCategoryId,
                    IsEsd = product.IsEsd,
                    OrderMinimumQuantity = product.OrderMinimumQuantity,
                    OrderMaximumQuantity = product.OrderMaximumQuantity,
                    QuantityStep = product.QuantityStep,
                    ManageInventoryMethodId = product.ManageInventoryMethodId,
                    UseMultipleWarehouses = product.UseMultipleWarehouses,
                    WarehouseId = product.WarehouseId,
                    Sku = product.Sku,
                    StockQuantity = product.StockQuantity,
                    ReservedQuantity = product.ReservedQuantity,
                    PlannedQuantity = product.PlannedQuantity,
                    MultiPacking = product.MultiPacking,
                    Packages = product.Packages,
                    Weight = product.Weight,
                    Length = product.Length,
                    Width = product.Width,
                    Height = product.Height,
                    DeliveryTimeId = product.DeliveryTimeId,
                    IsShipEnabled = product.IsShipEnabled,
                    IsFreeShipping = product.IsFreeShipping,
                    AdditionalShippingCharge = product.AdditionalShippingCharge,
                    CanReturn = product.CanReturn,
                    CustomsTariffNumber = product.CustomsTariffNumber,
                    Deleted = product.Deleted,
                    Status = product.Status,
                    CreatedBy = product.CreatedBy,
                    UpdatedBy = product.UpdatedBy,
                    CreatedDate = product.CreatedDate,
                    UpdatedDate = product.UpdatedDate,
                    CreatedByName = product.CreatedByName,
                    UpdatedByName = product.UpdatedByName,
                    OriginId = product.OriginId,
                    BrandId = product.BrandId,
                    ManufacturerId = product.ManufacturerId,
                    ProductTypeId = product.ProductTypeId,
                    CategoryRootId = product.CategoryRootId,
                    CategoryRoot = product.CategoryRoot,
                    AttributesJson = product.AttributesJson
                };
                return result;
            }
            else
            {
                var filter = new Dictionary<string, object>();
                filter.Add("productId", request.ProductId);
                var listStore = await _productStoreMappingRepository.GetListListBox(filter);
                var stores = await _storeRepository.GetById(listStore);
                var listGroupCategory = await _productGroupCategoryMappingRepository.GetListListBox(filter);
                var listCategoryMapping = await _productCategoryMappingRepository.GetListListBox(filter);
                var listCategory = await _categoryRepository.GetByIds(listCategoryMapping);
                var listTagMapping = await _productProductTagMappingRepository.GetListListBox(filter);
                var listTag = await _productTagRepository.GetByIds(listTagMapping);
                var listInventory = await _productInventoryRepository.GetListListBox(filter);
                var result = new ProductFullDto()
                {
                    Id = product.Id,
                    Code = product.Code,
                    ProductType = product.ProductType,
                    ForBuy = product.ForBuy,
                    ForSale = product.ForSale,
                    ForProduction = product.ForProduction,
                    Condition = product.Condition,
                    UnitId = product.UnitId,
                    UnitCode = product.UnitCode,
                    UnitType = product.UnitType,
                    Name = product.Name,
                    SourceLink = product.SourceLink,
                    SourceCode = product.SourceCode,
                    ShortDescription = product.ShortDescription,
                    FullDescription = product.FullDescription.HtmlStandardized(),
                    LimitedToStores = product.LimitedToStores,
                    IdGroupCategories = product.IdGroupCategories,
                    GroupCategories = product.GroupCategories,
                    Categories = product.Categories,
                    Origin = product.Origin,
                    Brand = product.Brand,
                    Manufacturer = product.Manufacturer,
                    ManufacturerNumber = product.ManufacturerNumber,
                    Image = product.Image,
                    Gtin = product.Gtin,
                    ProductCost = product.ProductCost,
                    CurrencyCost = product.CurrencyCost,
                    Price = product.Price,
                    HasTierPrices = product.HasTierPrices,
                    Currency = product.Currency,
                    IsTaxExempt = product.IsTaxExempt,
                    TaxCategoryId = product.TaxCategoryId,
                    IsEsd = product.IsEsd,
                    OrderMinimumQuantity = product.OrderMinimumQuantity,
                    OrderMaximumQuantity = product.OrderMaximumQuantity,
                    QuantityStep = product.QuantityStep,
                    ManageInventoryMethodId = product.ManageInventoryMethodId,
                    UseMultipleWarehouses = product.UseMultipleWarehouses,
                    WarehouseId = product.WarehouseId,
                    Sku = product.Sku,
                    StockQuantity = product.StockQuantity,
                    ReservedQuantity = product.ReservedQuantity,
                    PlannedQuantity = product.PlannedQuantity,
                    MultiPacking = product.MultiPacking,
                    Packages = product.Packages,
                    Weight = product.Weight,
                    Length = product.Length,
                    Width = product.Width,
                    Height = product.Height,
                    DeliveryTimeId = product.DeliveryTimeId,
                    IsShipEnabled = product.IsShipEnabled,
                    IsFreeShipping = product.IsFreeShipping,
                    AdditionalShippingCharge = product.AdditionalShippingCharge,
                    CanReturn = product.CanReturn,
                    CustomsTariffNumber = product.CustomsTariffNumber,
                    Deleted = product.Deleted,
                    Status = product.Status,
                    CreatedBy = product.CreatedBy,
                    UpdatedBy = product.UpdatedBy,
                    CreatedDate = product.CreatedDate,
                    UpdatedDate = product.UpdatedDate,
                    CreatedByName = product.CreatedByName,
                    UpdatedByName = product.UpdatedByName,
                    OriginId = product.OriginId,
                    BrandId = product.BrandId,
                    ManufacturerId = product.ManufacturerId,
                    ProductTypeId = product.ProductTypeId,
                    CategoryRootId = product.CategoryRootId,
                    CategoryRoot = product.CategoryRoot,
                    ListStore = listStore.Select(x => new ProductMappingDto()
                    {
                        Value = x.StoreId,
                    }
                    ).ToList(),
                    ListGroupCategory = listGroupCategory.Select(x => new ProductMappingDto()
                    {
                        Value = x.GroupCategoryId,
                    }
                    ).ToList(),
                    ListProductInventory = listInventory.Select(x => new ProductInventoryDto()
                    {
                        SpecificationCode10 = x.SpecificationCode10,
                        PlannedQuantity = x.PlannedQuantity,
                        ReservedQuantity = x.ReservedQuantity,
                        StockQuantity = x.StockQuantity,
                        WarehouseId = x.WarehouseId,
                        WarehouseCode = x.Warehouse.Code,
                        WarehouseName = x.Warehouse.Name,
                        SpecificationCode1 = x.SpecificationCode1,
                        SpecificationCode2 = x.SpecificationCode2,
                        SpecificationCode3 = x.SpecificationCode3,
                        SpecificationCode4 = x.SpecificationCode4,
                        SpecificationCode5 = x.SpecificationCode5,
                        SpecificationCode6 = x.SpecificationCode6,
                        SpecificationCode7 = x.SpecificationCode7,
                        SpecificationCode8 = x.SpecificationCode8,
                        SpecificationCode9 = x.SpecificationCode9,
                        UnitId = x.Unit?.Id,
                        UnitName = x.Unit?.Name,
                        UnitCode = x.Unit?.Code,
                        Id = x.Id
                    }
                    ).ToList(),
                    ListCategory = listCategory.Select(x => new MappingProductCategoriesDto()
                    {
                        Value = x.Id,
                        Label = x.FullName,
                        DisplayOrder = x.DisplayOrder,
                        GroupCategoryId = x.GroupCategoryId
                    }
                    ).OrderBy(x => x.DisplayOrder).ToList(),
                    ListTag = listTag.ToList().Select(x => x.Name).ToList(),
                    Stores = String.Join(", ", stores.ToList().Select(x => x.Name).ToArray()),
                    Combinations = product.Combinations,
                    Tags = product.Tags,
                    AttributesJson = product.AttributesJson
                };
                return result;
            }

        }
        public async Task<ProductFullDto> Handle(ProductQueryByProductCode request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByCode(request.Code);
            var result = new ProductFullDto()
            {
                Id = product.Id,
                Code = product.Code,
                ProductType = product.ProductType,
                ForBuy = product.ForBuy,
                ForSale = product.ForSale,
                ForProduction = product.ForProduction,
                Condition = product.Condition,
                UnitId = product.UnitId,
                UnitCode = product.UnitCode,
                UnitType = product.UnitType,
                UnitName = product.Unit != null ? product.Unit.Name : "",
                Name = product.Name,
                SourceLink = product.SourceLink,
                SourceCode = product.SourceCode,
                ShortDescription = product.ShortDescription,
                FullDescription = product.FullDescription,
                LimitedToStores = product.LimitedToStores,
                IdGroupCategories = product.IdGroupCategories,
                GroupCategories = product.GroupCategories,
                Categories = product.Categories,
                Origin = product.Origin,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer,
                ManufacturerNumber = product.ManufacturerNumber,
                Image = product.Image,
                Gtin = product.Gtin,
                ProductCost = product.ProductCost,
                CurrencyCost = product.CurrencyCost,
                Price = product.Price,
                HasTierPrices = product.HasTierPrices,
                Currency = product.Currency,
                IsTaxExempt = product.IsTaxExempt,
                TaxCategoryId = product.TaxCategoryId,
                IsEsd = product.IsEsd,
                OrderMinimumQuantity = product.OrderMinimumQuantity,
                OrderMaximumQuantity = product.OrderMaximumQuantity,
                QuantityStep = product.QuantityStep,
                ManageInventoryMethodId = product.ManageInventoryMethodId,
                UseMultipleWarehouses = product.UseMultipleWarehouses,
                WarehouseId = product.WarehouseId,
                Sku = product.Sku,
                StockQuantity = product.StockQuantity,
                ReservedQuantity = product.ReservedQuantity,
                PlannedQuantity = product.PlannedQuantity,
                MultiPacking = product.MultiPacking,
                Packages = product.Packages,
                Weight = product.Weight,
                Length = product.Length,
                Width = product.Width,
                Height = product.Height,
                DeliveryTimeId = product.DeliveryTimeId,
                IsShipEnabled = product.IsShipEnabled,
                IsFreeShipping = product.IsFreeShipping,
                AdditionalShippingCharge = product.AdditionalShippingCharge,
                CanReturn = product.CanReturn,
                CustomsTariffNumber = product.CustomsTariffNumber,
                Deleted = product.Deleted,
                Status = product.Status,
                CreatedBy = product.CreatedBy,
                UpdatedBy = product.UpdatedBy,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                CreatedByName = product.CreatedByName,
                UpdatedByName = product.UpdatedByName,
                OriginId = product.OriginId,
                BrandId = product.BrandId,
                ManufacturerId = product.ManufacturerId,
                ProductTypeId = product.ProductTypeId,
                CategoryRootId = product.CategoryRootId,
                CategoryRoot = product.CategoryRoot,
                Combinations = product.Combinations,
                Tags = product.Tags,
                AttributesJson = product.AttributesJson,
            };
            return result;
        }
        public async Task<List<ProductsInventoryDto>> Handle(ProductInventoryQueryByCode request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.Filter(request.ProductList);
            var productsInventory = await _productInventoryRepository.Filter(products);
            var warehouses = await _warehouseRepository.Filter(productsInventory);
            var result = productsInventory.Select(item =>
            {
                var product = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                var warehouse = warehouses.Where(x => x.Id == item.WarehouseId).FirstOrDefault();
                return new ProductsInventoryDto()
                {
                    ProductId = product.Id,
                    ProductCode = product.Code,
                    ProductName = product.Name,
                    WarehouseId = warehouse.Id,
                    WarehouseName = warehouse.Name,
                    StockQuantity = item.StockQuantity,
                    ReservedQuantity = item.ReservedQuantity,
                    PlannedQuantity = item.PlannedQuantity,
                };
            }
            ).ToList();
            return result;
        }
        public async Task<List<SP_GET_INVENTORY_BY_LISTID>> Handle(ProductInventoryQueryByListId request, CancellationToken cancellationToken)
        {
            var stock = await _pimContextProcedures.SP_GET_INVENTORY_BY_LISTIDAsync(request.ProductList);
            var result = stock.Select(item =>
            {
                return new SP_GET_INVENTORY_BY_LISTID()
                {
                    Id = item.Id,
                    Code = item.Code,
                    StockQuantity = item.StockQuantity,
                    ReservedQuantity = item.ReservedQuantity,
                    PlannedQuantity = item.PlannedQuantity,
                };
            }
            ).ToList();
            return result;
        }
        public async Task<List<SP_GET_INVENTORY_DETAIL>> Handle(ProductListWarehouseQueryById request, CancellationToken cancellationToken)
        {
            var stock = await _pimContextProcedures.SP_GET_INVENTORY_DETAILAsync(request.ProductId);
            var result = stock.Select(item =>
            {
                return new SP_GET_INVENTORY_DETAIL()
                {
                    Id = item.Id,
                    WarehouseId = item.WarehouseId,
                    Code = item.Code,
                    Name = item.Name,
                    ProductId = item.ProductId,
                    StockQuantity = item.StockQuantity,
                    ReservedQuantity = item.ReservedQuantity,
                    PlannedQuantity = item.PlannedQuantity,
                };
            }
            ).ToList();
            return result;
        }

        public async Task<SP_GET_INVENTORY_BY_ID> Handle(ProductStockQueryById request, CancellationToken cancellationToken)
        {
            OutputParameter<Guid?> id = new OutputParameter<Guid?>();
            OutputParameter<string> code = new OutputParameter<string>();
            OutputParameter<int?> stockQuantity = new OutputParameter<int?>();
            OutputParameter<int?> reservedQuantity = new OutputParameter<int?>();
            OutputParameter<int?> plannedQuantity = new OutputParameter<int?>();


            var stock = await _pimContextProcedures.SP_GET_INVENTORY_BY_PRODUCTIDAsync(request.ProductId, id, code, stockQuantity, reservedQuantity, plannedQuantity);

            return new SP_GET_INVENTORY_BY_ID()
            {
                Id = id.Value,
                Code = code.Value,
                StockQuantity = stockQuantity.Value,
                ReservedQuantity = reservedQuantity.Value,
                PlannedQuantity = plannedQuantity.Value
            };
        }

        public async Task<List<SP_GET_PRODUCT_PRICE_BY_LISTID>> Handle(ProductQueryPrice request, CancellationToken cancellationToken)
        {
            var stock = await _pimContextProcedures.SP_GET_PRODUCT_PRICE_BY_LISTIDAsync(request.ProductList);
            var result = stock.Select(item =>
            {
                return new SP_GET_PRODUCT_PRICE_BY_LISTID()
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name,
                    Price = item.Price,
                    UnitId = item.UnitId,
                    Currency = item.Currency
                };
            }
            ).ToList();
            return result;
        }

        public async Task<List<SP_GET_INVENTORY_DETAIL_BY_LISTID>> Handle(ProductQueryInventoryDetailByListId request, CancellationToken cancellationToken)
        {
            var stock = await _pimContextProcedures.SP_GET_INVENTORY_DETAIL_BY_LISTIDAsync(request.ProductList);
            var result = stock.Select(item =>
            {
                return new SP_GET_INVENTORY_DETAIL_BY_LISTID()
                {
                    Id = item.Id,
                    WarehouseId = item.WarehouseId,
                    Code = item.Code,
                    Name = item.Name,
                    ProductId = item.ProductId,
                    StockQuantity = item.StockQuantity,
                    ReservedQuantity = item.ReservedQuantity,
                    PlannedQuantity = item.PlannedQuantity
                };
            }
            ).ToList();
            return result;
        }
        public async Task<PagedResult<List<ProductDto>>> Handle(ProductPagingFilterQuery request, CancellationToken cancellationToken)
        {
            var response = new PagedResult<List<ProductDto>>();

            var fopRequest = FopExpressionBuilder<Product>.Build(request.Filter, request.Order, request.PageNumber, request.PageSize);

            var (datas, count) = await _productRepository.Filter(request.Keyword, fopRequest);
            var result = datas.Select(product =>
            {
                return new ProductDto()
                {
                    Id = product.Id,
                    Image = product.Image,
                    Name = product.Name,
                    Code = product.Code,
                    Price = product.Price,
                    ProductType = product.ProductType,
                    Categories = product.Categories,
                    IdCategories = product.IdCategories!,
                    LimitedToStores = product.LimitedToStores,
                    IdGroupCategories = product.IdGroupCategories,
                    GroupCategories = product.GroupCategories!,
                    WarehouseId = product.WarehouseId,
                    Warehouse = product.Warehouse,
                    Brand = product.Brand,
                    Manufacturer = product.Manufacturer,
                    Sku = product.Sku,
                    Gtin = product.Gtin,
                    Status = product.Status,
                    UnitId = product.UnitId,
                    UnitType = product.UnitType,
                    UnitName = product.Unit?.Name ?? "",
                    UnitCode = product.Unit?.Code ?? "",
                    CurrencyCost = product.CurrencyCost,
                    Currency = product.Currency,
                    OriginId = product.OriginId,
                    Origin = product.Origin,
                    OriginCode = product.OriginNavigation?.Code,
                    CategoryRootId = product.CategoryRootId,
                    CategoryRoot = product.CategoryRoot,
                    MultiPacking = product.MultiPacking,
                    Packages = product.Packages,
                    Weight = product.Weight,
                    Length = product.Length,
                    Width = product.Width,
                    Height = product.Height,
                    CreatedBy = product.CreatedBy,
                    CreatedByName = product.CreatedByName,
                    CreatedDate = product.CreatedDate,
                    UpdatedBy = product.UpdatedBy,
                    UpdatedByName = product.UpdatedByName,
                    UpdatedDate = product.UpdatedDate,
                    AttributesJson = product.AttributesJson,
                    SourchCode = product.SourceCode,
                    SourchLink = product.SourceLink,
                    StockQuantity = product.ProductInventory.Sum(x => x.StockQuantity),
                    TaxCategoryId = product.TaxCategoryId,
                    IsSpec = product.ProductSpecificationCode.Count() > 0 ? true : false
                };
            }
            ).ToList();
            response.Items = result;
            response.TotalCount = count;
            response.PageNumber = request.PageNumber;
            response.PageSize = request.PageSize;
            return response;
        }
        public async Task<IEnumerable<ListBoxDto>> Handle(ProductQueryListBox request, CancellationToken cancellationToken)
        {
            var filter = new Dictionary<string, object>();
            if (request.QueryParams.Status != null)
            {
                filter.Add("status", request.QueryParams.Status);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.ProductTypeId))
            {
                filter.Add("productType", request.QueryParams.ProductTypeId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.CategoryRootId))
            {
                filter.Add("categoryRoot", request.QueryParams.CategoryRootId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.OriginId))
            {
                filter.Add("origin", request.QueryParams.OriginId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.UnitId))
            {
                filter.Add("unit", request.QueryParams.UnitId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.TaxCategoryId))
            {
                filter.Add("taxCategory", request.QueryParams.TaxCategoryId);
            }
            if (!String.IsNullOrEmpty(request.QueryParams.BrandId))
            {
                filter.Add("brand", request.QueryParams.BrandId);
            }
            var products = await _productRepository.GetListListBox(filter, request.Keyword);
            var result = products.Select(x => new ListBoxDto()
            {
                Value = x.Id,
                Label = x.Name
            });
            return result;
        }

        public async Task<ProductDuplicateDto> Handle(ProductQueryByCode request, CancellationToken cancellationToken)
        {
            var productCopy = await _productRepository.GetByCode(request.Code);

            var result = new ProductDuplicateDto()
            {
                Id = productCopy.Id,
            };
            return result;

        }

        public async Task<IEnumerable<ProductDto>> Handle(ProductQueryByListCode request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.Filter(request.Codes);

            var result = products.Select(product => new ProductDto
            {
                Id = product.Id,
                Image = product.Image,
                Name = product.Name,
                Code = product.Code,
                Price = product.Price,
                ProductType = product.ProductType,
                Categories = product.Categories,
                IdCategories = product.IdCategories,
                LimitedToStores = product.LimitedToStores,
                IdGroupCategories = product.IdGroupCategories,
                GroupCategories = product.GroupCategories,
                WarehouseId = product.WarehouseId,
                Warehouse = product.Warehouse,
                Brand = product.Brand,
                Manufacturer = product.Manufacturer,
                Sku = product.Sku,
                Gtin = product.Gtin,
                Status = product.Status,
                UnitId = product.UnitId,
                UnitType = product.UnitType,
                UnitName = product.Unit?.Name ?? "",
                UnitCode = product.Unit?.Code ?? "",
                CurrencyCost = product.CurrencyCost,
                Currency = product.Currency,
                OriginId = product.OriginId,
                Origin = product.Origin,
                CategoryRootId = product.CategoryRootId,
                CategoryRoot = product.CategoryRoot,
                MultiPacking = product.MultiPacking,
                Packages = product.Packages,
                Weight = product.Weight,
                Length = product.Length,
                Width = product.Width,
                Height = product.Height,
                CreatedBy = product.CreatedBy,
                CreatedByName = product.CreatedByName,
                CreatedDate = product.CreatedDate,
                UpdatedBy = product.UpdatedBy,
                UpdatedByName = product.UpdatedByName,
                UpdatedDate = product.UpdatedDate,
                AttributesJson = product.AttributesJson,
                StockQuantity = product.ProductInventory.Sum(x => x.StockQuantity),
                TaxCategoryId = product.TaxCategoryId,
                IsSpec = product.ProductSpecificationCode.Count() > 0 ? true : false
            });

            return result;
        }
    }
}
