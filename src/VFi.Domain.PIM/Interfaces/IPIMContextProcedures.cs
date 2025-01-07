using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace VFi.Domain.PIM.Interfaces
{
    public partial interface IPIMContextProcedures
    {
        Task<List<SP_GET_INVENTORY_BY_LISTCODE>> SP_GET_INVENTORY_BY_LISTCODEAsync(string listProductCode, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_INVENTORY_BY_LISTID>> SP_GET_INVENTORY_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> SP_GET_INVENTORY_BY_PRODUCTIDAsync(Guid? productId, OutputParameter<Guid?> id, OutputParameter<string> code, OutputParameter<int?> stockQuantity, OutputParameter<int?> reservedQuantity, OutputParameter<int?> plannedQuantity, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_INVENTORY_DETAIL>> SP_GET_INVENTORY_DETAILAsync(Guid? productId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_PRODUCT_BY_STOREID>> SP_GET_PRODUCT_BY_STOREIDAsync(Guid? storeId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_INVENTORY_DETAIL_BY_LISTID>> SP_GET_INVENTORY_DETAIL_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_PRODUCT_PRICE_BY_LISTID>> SP_GET_PRODUCT_PRICE_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_PRODUCT_PRICE_COST_BY_LISTID>> SP_GET_PRODUCT_PRICE_COST_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> SP_COPY_PRODUCTAsync(Guid? productId, string new_ProductCode, string new_ProductName, int? new_Status, Guid? createdBy, string? createdByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_COUNT_PRODUCTS>> SP_COUNT_PRODUCTSAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_TOP_PRODUCTS_INVENTORY>> SP_GET_TOP_PRODUCTS_INVENTORYAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_TOP_MANUFACTURER>> SP_GET_TOP_MANUFACTURERAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_TOP_PRODUCT_BRAND>> SP_GET_TOP_PRODUCT_BRANDAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_TOP_CATEGORY>> SP_GET_TOP_CATEGORYAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_COUNT_PRODUCT_BY_PRODUCTTYPE>> SP_COUNT_PRODUCT_BY_PRODUCTTYPEAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<List<SP_GET_TOP_NEW_PRODUCT>> SP_GET_TOP_NEW_PRODUCTAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> SP_ADD_PRODUCT_VARIANTAsync(Guid? Id, string? Code, string? Name, int? Status, string? AttributesJson, string? Sku, string? ManufacturerNumber, string? Gtin, decimal? Price, string? Currency, Guid? DeliveryTimeId, Guid? UnitId, string? UnitCode, string? UnitType, int? ManageInventoryMethodId, bool? MultiPacking, int? Packages, decimal? Weight, decimal? Length, decimal? Width, decimal? Height, Guid? ParentId, Guid? ActionBy, string ActionByName, DataTable ListInventory, DataTable ListPackage, DataTable ListMedia, DataTable ListProductSpecificationCode, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> SP_CREATE_ALL_VARIANTAsync(Guid? Id, string ListVariant, Guid? ActionBy, string? ActionByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> SP_ADD_SIMPLE_PRODUCTAsync(Guid? Id, string Code, string ProductType, int? Condition, string UnitType, string UnitCode, string Name, string SourceLink, string ShortDescription, string FullDescription, string LimitedToStores, string Channel, string Channel_Category, string Origin, string Brand, string Manufacturer, string ManufacturerNumber, string Image, string Images, string Gtin, decimal? ProductCost, string CurrencyCost, decimal? Price, string Currency, bool? IsTaxExempt, int? Tax, int? OrderMinimumQuantity, int? OrderMaximumQuantity, bool? IsShipEnabled, bool? IsFreeShipping, string ProductTag, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
