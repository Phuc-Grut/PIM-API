
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Infra.PIM.Context
{
    public partial class PIMContextProcedures  
    {
          

        public async Task<int> SP_CREATE_PRODUCT_TOPIC_DETAILAsync(Guid? id, string productType, Guid? productTopicId, string productTopic, string code, int? condition, string unit, string name, string sourceLink, string sourceCode, string shortDescription, string fullDescription, string origin, string brand, string manufacturer, string image, string images, decimal? price, string currency, int? status, string tags, DateTime? exp, decimal? bidPrice, int? tax, string channel, Guid? createdBy, string createdByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                    ParameterName = "id",
                    Value = id ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "productType",
                    Size = 500,
                    Value = productType ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "productTopicId",
                    Value = productTopicId ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "productTopic",
                    Size = 50,
                    Value = productTopic ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "code",
                    Size = 50,
                    Value = code ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "condition",
                    Value = condition ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "unit",
                    Size = 100,
                    Value = unit ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "name",
                    Size = 800,
                    Value = name ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "sourceLink",
                    Size = 500,
                    Value = sourceLink ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "sourceCode",
                    Size = 50,
                    Value = sourceCode ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "shortDescription",
                    Size = 8000,
                    Value = shortDescription ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "fullDescription",
                    Size = -1,
                    Value = fullDescription ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "origin",
                    Size = 510,
                    Value = origin ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "brand",
                    Size = 510,
                    Value = brand ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "manufacturer",
                    Size = 510,
                    Value = manufacturer ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "image",
                    Size = 1000,
                    Value = image ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "images",
                    Size = -1,
                    Value = images ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "price",
                    Precision = 19,
                    Scale = 4,
                    Value = price ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Money,
                },
                new SqlParameter
                {
                    ParameterName = "currency",
                    Size = 50,
                    Value = currency ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
                },
                new SqlParameter
                {
                    ParameterName = "status",
                    Value = status ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "tags",
                    Size = 2000,
                    Value = tags ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                new SqlParameter
                {
                    ParameterName = "exp",
                    Value = exp ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
                },
                new SqlParameter
                {
                    ParameterName = "bidPrice",
                    Precision = 19,
                    Scale = 4,
                    Value = bidPrice ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Money,
                },
                new SqlParameter
                {
                    ParameterName = "tax",
                    Value = tax ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.Int,
                },
                new SqlParameter
                {
                    ParameterName = "channel",
                    Size = 50,
                    Value = channel ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.VarChar,
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
                    Size = 510,
                    Value = createdByName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [pim].[SP_CREATE_PRODUCT_TOPIC_DETAIL] @id, @productType, @productTopicId, @productTopic, @code, @condition, @unit, @name, @sourceLink, @sourceCode, @shortDescription, @fullDescription, @origin, @brand, @manufacturer, @image, @images, @price, @currency, @status, @tags, @exp, @bidPrice, @tax, @channel, @createdBy, @createdByName", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
        public async Task<int> SP_PUBLISH_PRODUCT_TOPIC_DETAILAsync(Guid? id, DateTime? publishDate, Guid? createdBy, string createdByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default)
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
                    ParameterName = "id",
                    Value = id ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                },
                new SqlParameter
                {
                    ParameterName = "publishDate",
                    Value = publishDate ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.DateTime,
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
                    Size = 510,
                    Value = createdByName ?? Convert.DBNull,
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                },
                parameterreturnValue,
            };
            var _ = await _context.Database.ExecuteSqlRawAsync("EXEC @returnValue = [pim].[SP_PUBLISH_PRODUCT_TOPIC_DETAIL] @id, @publishDate, @createdBy, @createdByName", sqlParameters, cancellationToken);

            returnValue?.SetValue(parameterreturnValue.Value);

            return _;
        }
    }
}
