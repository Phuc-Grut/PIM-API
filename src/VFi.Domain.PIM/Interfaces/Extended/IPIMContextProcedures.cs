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
       
        Task<int> SP_CREATE_PRODUCT_TOPIC_DETAILAsync(Guid? id, string productType, Guid? productTopicId, string productTopic, string code, int? condition, string unit, string name, string sourceLink, string sourceCode, string shortDescription, string fullDescription, string origin, string brand, string manufacturer, string image, string images, decimal? price, string currency, int? status, string tags, DateTime? exp, decimal? bidPrice, int? tax, string channel, Guid? createdBy, string createdByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> SP_PUBLISH_PRODUCT_TOPIC_DETAILAsync(Guid? id, DateTime? publishDate, Guid? createdBy, string createdByName, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
