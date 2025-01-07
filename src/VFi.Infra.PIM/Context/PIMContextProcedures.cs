
using System.Data;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace VFi.Infra.PIM.Context
{
    public partial class PIMContextProcedures : IPIMContextProcedures
    {
        private readonly SqlCoreContext _context;

        public PIMContextProcedures(IServiceProvider serviceProvider)
        {
            //var scope = serviceProvider.CreateScope();
            //_context = scope.ServiceProvider.GetRequiredService<SqlCoreContext>();
            _context = serviceProvider.GetRequiredService<SqlCoreContext>();
        }


        public async Task<int> SP_COPY_PRODUCTAsync(Guid? productId, string new_ProductCode, string new_ProductName, int? new_Status, Guid? createdBy, string? createdByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "productId",
                    Value = productId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "new_ProductCode",
                    Size = 50,
                    Value = new_ProductCode ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "new_ProductName",
                    Size = 510,
                    Value = new_ProductName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "new_Status",
                    Value = new_Status ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "createdBy",
                    Value = createdBy ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "createdByName",
                    Value = createdByName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [pim].[SP_COPY_PRODUCT] @productId, @new_ProductCode, @new_ProductName, @new_Status, @createdBy, @createdByName", sqlParameters, cancellationToken);



            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<List<SP_GET_INVENTORY_BY_LISTCODE>> SP_GET_INVENTORY_BY_LISTCODEAsync(string listProductCode, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "listProductCode",
                    Size = 4000,
                    Value = listProductCode ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_INVENTORY_BY_LISTCODE>("EXEC @returnValue = [pim].[SP_GET_INVENTORY_BY_LISTCODE] @listProductCode", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<List<SP_GET_INVENTORY_BY_LISTID>> SP_GET_INVENTORY_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "listProductId",
                    Size = 4000,
                    Value = listProductId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_INVENTORY_BY_LISTID>("EXEC @returnValue = [pim].[SP_GET_INVENTORY_BY_LISTID] @listProductId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<int> SP_GET_INVENTORY_BY_PRODUCTIDAsync(Guid? productId, OutputParameter<Guid?> id, OutputParameter<string> code, OutputParameter<int?> stockQuantity, OutputParameter<int?> reservedQuantity, OutputParameter<int?> plannedQuantity, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterid = new SqlParameter
            {
                ParameterName = "id",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = id?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
            };
            var parametercode = new SqlParameter
            {
                ParameterName = "code",
                Size = 50,
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = code?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.VarChar,
            };
            var parameterstockQuantity = new SqlParameter
            {
                ParameterName = "stockQuantity",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = stockQuantity?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreservedQuantity = new SqlParameter
            {
                ParameterName = "reservedQuantity",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = reservedQuantity?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterplannedQuantity = new SqlParameter
            {
                ParameterName = "plannedQuantity",
                Direction = System.Data.ParameterDirection.InputOutput,
                Value = plannedQuantity?._value ?? Convert.DBNull,
                SqlDbType = System.Data.SqlDbType.Int,
            };
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "productId",
                    Value = productId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                parameterid,
                parametercode,
                parameterstockQuantity,
                parameterreservedQuantity,
                parameterplannedQuantity,
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [pim].[SP_GET_INVENTORY_BY_PRODUCTID] @productId,@id OUTPUT, @code OUTPUT, @stockQuantity OUTPUT, @reservedQuantity OUTPUT, @plannedQuantity OUTPUT", sqlParameters, cancellationToken);

            id.SetValue(parameterid.Value);
            code.SetValue(parametercode.Value);
            stockQuantity.SetValue(parameterstockQuantity.Value);
            reservedQuantity.SetValue(parameterreservedQuantity.Value);
            plannedQuantity.SetValue(parameterplannedQuantity.Value);
            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<List<SP_GET_INVENTORY_DETAIL>> SP_GET_INVENTORY_DETAILAsync(Guid? productId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "productId",
                    Value = productId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_INVENTORY_DETAIL>("EXEC @returnValue = [pim].[SP_GET_INVENTORY_DETAIL] @productId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<List<SP_GET_PRODUCT_BY_STOREID>> SP_GET_PRODUCT_BY_STOREIDAsync(Guid? storeId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "storeId",
                    Value = storeId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_PRODUCT_BY_STOREID>("EXEC @returnValue = [pim].[SP_GET_PRODUCT_BY_STOREID] @storeId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<List<SP_GET_INVENTORY_DETAIL_BY_LISTID>> SP_GET_INVENTORY_DETAIL_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "listProductId",
                    Size = 4000,
                    Value = listProductId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_INVENTORY_DETAIL_BY_LISTID>("EXEC @returnValue = [pim].[SP_GET_INVENTORY_DETAIL_BY_LISTID] @listProductId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_GET_PRODUCT_PRICE_BY_LISTID>> SP_GET_PRODUCT_PRICE_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "listProductId",
                    Size = 4000,
                    Value = listProductId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_PRODUCT_PRICE_BY_LISTID>("EXEC @returnValue = [pim].[SP_GET_PRODUCT_PRICE_BY_LISTID] @listProductId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<List<SP_GET_PRODUCT_PRICE_COST_BY_LISTID>> SP_GET_PRODUCT_PRICE_COST_BY_LISTIDAsync(string listProductId, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "listProductId",
                    Size = 4000,
                    Value = listProductId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_PRODUCT_PRICE_COST_BY_LISTID>("EXEC @returnValue = [pim].[SP_GET_PRODUCT_PRICE_COST_BY_LISTID] @listProductId", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_COUNT_PRODUCTS>> SP_COUNT_PRODUCTSAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_COUNT_PRODUCTS>("EXEC @returnValue = [pim].[SP_COUNT_PRODUCTS]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_GET_TOP_PRODUCTS_INVENTORY>> SP_GET_TOP_PRODUCTS_INVENTORYAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_TOP_PRODUCTS_INVENTORY>("EXEC @returnValue = [pim].[SP_GET_TOP_PRODUCTS_INVENTORY]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_GET_TOP_MANUFACTURER>> SP_GET_TOP_MANUFACTURERAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_TOP_MANUFACTURER>("EXEC @returnValue = [pim].[SP_GET_TOP_MANUFACTURER]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_GET_TOP_PRODUCT_BRAND>> SP_GET_TOP_PRODUCT_BRANDAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_TOP_PRODUCT_BRAND>("EXEC @returnValue = [pim].[SP_GET_TOP_PRODUCT_BRAND]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_GET_TOP_CATEGORY>> SP_GET_TOP_CATEGORYAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_TOP_CATEGORY>("EXEC @returnValue = [pim].[SP_GET_TOP_CATEGORY]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_COUNT_PRODUCT_BY_PRODUCTTYPE>> SP_COUNT_PRODUCT_BY_PRODUCTTYPEAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_COUNT_PRODUCT_BY_PRODUCTTYPE>("EXEC @returnValue = [pim].[SP_COUNT_PRODUCT_BY_PRODUCTTYPE]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<List<SP_GET_TOP_NEW_PRODUCT>> SP_GET_TOP_NEW_PRODUCTAsync(OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                parameterreturnValue,
            };
            var _ = await _context.SqlQueryAsync<SP_GET_TOP_NEW_PRODUCT>("EXEC @returnValue = [pim].[SP_GET_TOP_NEW_PRODUCT]", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<int> SP_ADD_PRODUCT_VARIANTAsync(Guid? Id, string? Code, string? Name, int? Status, string? AttributesJson, string? Sku, string? ManufacturerNumber, string? Gtin, decimal? Price, string? Currency, Guid? DeliveryTimeId, Guid? UnitId, string? UnitCode, string? UnitType, int? ManageInventoryMethodId, bool? MultiPacking, int? Packages, decimal? Weight, decimal? Length, decimal? Width, decimal? Height, Guid? ParentId, Guid? ActionBy, string ActionByName, DataTable ListInventory, DataTable ListPackage, DataTable ListMedia, DataTable ListProductSpecificationCode, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "Id",
                    Value = Id ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "Code",
                    Size = 50,
                    Value = Code ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Name",
                    Size = 400,
                    Value = Name ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Status",
                    Value = Status ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "AttributesJson",
                    Size= int.MaxValue,
                    Value = AttributesJson ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Sku",
                    Size = 400,
                    Value = Sku ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ManufacturerNumber",
                    Size = 400,
                    Value = ManufacturerNumber ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Gtin",
                    Size = 400,
                    Value = Gtin ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Price",
                    Value = Price ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Money,
                },
                new SqlParameter
                {
                    ParameterName = "Currency",
                    Size = 50,
                    Value = Currency ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "DeliveryTimeId",
                    Value = DeliveryTimeId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "UnitId",
                    Value = UnitId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "UnitCode",
                    Size = 50,
                    Value = UnitCode ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "UnitType",
                    Size = 50,
                    Value = UnitType ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ManageInventoryMethodId",
                    Value = ManageInventoryMethodId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "MultiPacking",
                    Value = MultiPacking ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "Packages",
                    Value = Packages ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "Weight",
                    Value = Weight ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Decimal,
                },
                new SqlParameter
                {
                    ParameterName = "Length",
                    Value = Length ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Decimal,
                },
                new SqlParameter
                {
                    ParameterName = "Width",
                    Value = Width ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Decimal,
                },
                new SqlParameter
                {
                    ParameterName = "Height",
                    Value = Height ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Decimal,
                },
                new SqlParameter
                {
                    ParameterName = "ParentId",
                    Value = ParentId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "ActionBy",
                    Value = ActionBy ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "ActionByName",
                    Size = 500,
                    Value = ActionByName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ListInventory",
                    Value = ListInventory ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Structured,
                    TypeName = "[pim].[T_ProductInventory]",
                },
                new SqlParameter
                {
                    ParameterName = "ListPackage",
                    Value = ListPackage ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Structured,
                    TypeName = "[pim].[T_ProductPackage]",
                },
                new SqlParameter
                {
                    ParameterName = "ListMedia",
                    Value = ListMedia ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Structured,
                    TypeName = "[pim].[T_ProductMedia]",
                },
                new SqlParameter
                {
                    ParameterName = "ListProductSpecificationCode",
                    Value = ListMedia ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Structured,
                    TypeName = "[pim].[T_ProductSpecificationCode]",
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [pim].[SP_ADD_PRODUCT_VARIANT] @Id, @Code, @Name, @Status, @AttributesJson, @Sku, @ManufacturerNumber, @Gtin, @Price, @Currency, @DeliveryTimeId, @UnitId, @UnitCode, @UnitType, @ManageInventoryMethodId, @MultiPacking, @Packages, @Weight, @Length, @Width, @Height, @ParentId, @ActionBy, @ActionByName, @ListInventory, @ListPackage, @ListMedia, @ListProductSpecificationCode", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<int> SP_CREATE_ALL_VARIANTAsync(Guid? Id, string ListVariant, Guid? ActionBy, string? ActionByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "Id",
                    Value = Id ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "ListVariant",
                    Size = int.MaxValue,
                    Value = ListVariant ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                 new SqlParameter
                {
                    ParameterName = "ActionBy",
                    Value = ActionBy ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "ActionByName",
                    Size = 500,
                    Value = ActionByName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [pim].[SP_CREATE_ALL_VARIANT] @Id, @ListVariant, @ActionBy, @ActionByName", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }

        public async Task<int> SP_ADD_SIMPLE_PRODUCTAsync(Guid? Id, string Code, string ProductType, int? Condition, string UnitType, string UnitCode, string Name, string SourceLink, string ShortDescription, string FullDescription, string LimitedToStores, string Channel, string Channel_Category, string Origin, string Brand, string Manufacturer, string ManufacturerNumber, string Image, string Images, string Gtin, decimal? ProductCost, string CurrencyCost, decimal? Price, string Currency, bool? IsTaxExempt, int? Tax, int? OrderMinimumQuantity, int? OrderMaximumQuantity, bool? IsShipEnabled, bool? IsFreeShipping, string ProductTag, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
        {
            var parameterreturnValue = new SqlParameter
            {
                ParameterName = "returnValue",
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Int,
            };

            var sqlParameters = new[]
            {
                new SqlParameter
                {
                    ParameterName = "Id",
                    Value = Id ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "Code",
                    Size = 50,
                    Value = Code ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ProductType",
                    Size = 50,
                    Value = ProductType ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Condition",
                    Value = Condition ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "UnitType",
                    Size = 50,
                    Value = UnitType ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "UnitCode",
                    Size = 50,
                    Value = UnitCode ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Name",
                    Size = 510,
                    Value = Name ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "SourceLink",
                    Size = 510,
                    Value = SourceLink ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ShortDescription",
                    Size = 8000,
                    Value = ShortDescription ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "FullDescription",
                    Value = FullDescription ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NText,
                },
                new SqlParameter
                {
                    ParameterName = "LimitedToStores",
                    Size = 500,
                    Value = LimitedToStores ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Channel",
                    Size = 50,
                    Value = Channel ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Channel_Category",
                    Size = 50,
                    Value = Channel_Category ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Origin",
                    Size = 50,
                    Value = Origin ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Brand",
                    Size = 50,
                    Value = Brand ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Manufacturer",
                    Size = 50,
                    Value = Manufacturer ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ManufacturerNumber",
                    Size = 50,
                    Value = ManufacturerNumber ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Image",
                    Size = 500,
                    Value = Image ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Images",
                    Size = 8000,
                    Value = Images ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Gtin",
                    Size = 50,
                    Value = Gtin ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "ProductCost",
                    Precision = 19,
                    Scale = 4,
                    Value = ProductCost ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Money,
                },
                new SqlParameter
                {
                    ParameterName = "CurrencyCost",
                    Size = 50,
                    Value = CurrencyCost ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "Price",
                    Precision = 19,
                    Scale = 4,
                    Value = Price ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Money,
                },
                new SqlParameter
                {
                    ParameterName = "Currency",
                    Size = 50,
                    Value = Currency ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "IsTaxExempt",
                    Value = IsTaxExempt ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "Tax",
                    Value = Tax ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "OrderMinimumQuantity",
                    Value = OrderMinimumQuantity ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "OrderMaximumQuantity",
                    Value = OrderMaximumQuantity ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "IsShipEnabled",
                    Value = IsShipEnabled ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "IsFreeShipping",
                    Value = IsFreeShipping ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Bit,
                },
                new SqlParameter
                {
                    ParameterName = "ProductTag",
                    Size = 2000,
                    Value = ProductTag ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [pim].[SP_ADD_SIMPLE_PRODUCT] @Id, @Code, @ProductType, @Condition, @UnitType, @UnitCode, @Name, @SourceLink, @ShortDescription, @FullDescription, @LimitedToStores, @Channel, @Channel_Category, @Origin, @Brand, @Manufacturer, @ManufacturerNumber, @Image, @Images, @Gtin, @ProductCost, @CurrencyCost, @Price, @Currency, @IsTaxExempt, @Tax, @OrderMinimumQuantity, @OrderMaximumQuantity, @IsShipEnabled, @IsFreeShipping, @ProductTag", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
