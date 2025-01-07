using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using VFi.Domain.PIM.Events;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.Domain.PIM.QueryModels;
using VFi.Infra.PIM.Context;
using VFi.NetDevPack.Context;
using VFi.NetDevPack.Data;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using StackExchange.Redis;
using static System.Net.Mime.MediaTypeNames;

namespace VFi.Infra.ACC.MassTransit.Consumers;

public class AddProductTopicConsumer : IConsumer<AddProductTopicQueueEvent>
{
    private readonly ILogger<AddProductTopicQueueEvent> _logger;
    private readonly IServiceProvider _serviceFactory;
    public AddProductTopicConsumer(ILogger<AddProductTopicQueueEvent> logger, IServiceProvider serviceFactory)
    {
        _logger = logger;
        _serviceFactory = serviceFactory;
    }


    public async Task Consume(ConsumeContext<AddProductTopicQueueEvent> context)
    {
        Random rnd = new Random();
        var msg = context.Message;
        //_logger.LogInformation(GetType().Name + JsonConvert.SerializeObject(msg));

        var contextUser = _serviceFactory.GetService(typeof(IContextUser)) as IContextUser;
        contextUser.SetEnvTenant(msg.Tenant, msg.Data_Zone, msg.Data);
        var store = _serviceFactory.GetService(typeof(IPIMContextProcedures)) as IPIMContextProcedures;
        var repository = _serviceFactory.GetService(typeof(IProductTopicRepository)) as IProductTopicRepository;
        var spider = _serviceFactory.GetService(typeof(ISpiderRepository)) as ISpiderRepository;

        var topic = await repository.GetByCode(msg.Topic);
        var querys = topic.ProductTopicQuery.Where(x=>x.Status==1);
        var listUpdate = new List<ProductTopicDetail>();
        foreach (var query in querys)
        {
            if (query.SourceCode.ToLower().Equals("auc"))
            {
                var pageQuery = 5;
                if (query.PageQuery.HasValue) pageQuery = query.PageQuery.Value;
                for (var p = 1; p <= pageQuery; p++)
                {
                    Thread.Sleep(rnd.Next(1000, 5000));
                    // var condition = 0;
                    // if(query.Condition.HasValue) condition= query.Condition.Value;
                    var searchData = await spider.AuctionSearch(query.Keyword, query.Category, query.BrandId, query.Seller, query.Condition, query.SortQuery, 50, p);
                    var pItems = searchData.Items;
                    if (pItems != null)
                        foreach (var item in pItems)
                        {

                            var i = new ProductTopicDetail()
                            {
                                Id = Guid.NewGuid(),
                                ProductType = "Mua hộ",
                                ProductTopicId = topic.Id,
                                ProductTopic = topic.Code,
                                Code = "auc-" + item.ProductCode + "-" + topic.Code,
                                Name = item.ProductName,
                                SourceLink = item.ProductLink,
                                SourceCode = item.ProductCode,
                                Image = item.PreviewImage,
                                //Images = string.Join(",", item.Images.Any() ? item.Images:""),
                                Price = item.BuyNowPrice,
                                Tax = item.Tax,
                                BidPrice = item.Price,
                                Currency = item.Currency,
                                Status = 2,
                                CreatedDate = DateTime.Now,
                                Exp = item.EndTime,
                                Channel = "AUC"
                            };
                            listUpdate.Add(i);
                            //await  store.SP_CREATE_PRODUCT_TOPIC_DETAILAsync(i.Id, i.ProductType, i.ProductTopicId,i.ProductTopic,
                            // i.Code, i.Condition, i.Unit, i.Name, i.SourceLink, i.SourceCode, i.ShortDescription,
                            // i.FullDescription, i.Origin, i.Brand, i.Manufacturer, i.Image, i.Images,
                            // i.Price, i.Currency, i.Status, i.Tags, i.Exp, i.BidPrice,
                            //i.Tax, i.Channel, i.CreatedBy, i.CreatedByName);

                        }
                }

            }
            if (query.SourceCode.ToLower().Equals("mer"))
            {
                var pageQuery = 1;
                if (query.PageQuery.HasValue) pageQuery = query.PageQuery.Value;
                for (var p = 1; p <= pageQuery; p++)
                {
                    Thread.Sleep(rnd.Next(1000, 5000));
                    var searchData = await spider.MercariSearch(query.Keyword, query.Category, query.BrandId, query.Seller, 100, p);
                    var pItems = searchData.Items;
                    if (pItems != null)
                        foreach (var item in pItems)
                        {
                            var i = new ProductTopicDetail()
                            {
                                Id = Guid.NewGuid(),
                                ProductType = "Mua hộ",
                                ProductTopicId = topic.Id,
                                ProductTopic = topic.Code,
                                Code = "mer-" + item.ProductCode + "-" + topic.Code,
                                Name = item.ProductName,
                                SourceLink = item.ProductLink,
                                SourceCode = item.ProductCode,
                                Image = item.PreviewImage,
                                //Images = string.Join(",", item.Images.Any() ? item.Images:""),
                                Price = item.Price,
                                Tax = item.Tax,
                                Currency = item.Currency,
                                Status = 2,
                                CreatedDate = DateTime.Now,
                                Exp = DateTime.Now.AddDays(4),
                                Channel = "MER"
                            };
                            listUpdate.Add(i);
                            //await store.SP_CREATE_PRODUCT_TOPIC_DETAILAsync(i.Id, i.ProductType, i.ProductTopicId, i.ProductTopic,
                            // i.Code, i.Condition, i.Unit, i.Name, i.SourceLink, i.SourceCode, i.ShortDescription,
                            // i.FullDescription, i.Origin, i.Brand, i.Manufacturer, i.Image, i.Images,
                            // i.Price, i.Currency, i.Status, i.Tags, i.Exp, i.BidPrice,
                            //i.Tax, i.Channel, i.CreatedBy, i.CreatedByName);

                        }
                }
            }

            if (query.SourceCode.ToLower().Equals("golf"))
            {
                var pageQuery = 1;
                if (query.PageQuery.HasValue) pageQuery = query.PageQuery.Value;
                for (var p = 1; p <= pageQuery; p++)
                {
                    Thread.Sleep(rnd.Next(1000, 5000));
                    var searchData = await spider.GolfPartnerSearch(query.Keyword, query.Category, "1", query.Seller, 100, p);
                    var pItems = searchData.Items;
                    if (pItems != null)
                        foreach (var item in pItems)
                        {
                            var i = new ProductTopicDetail()
                            {
                                Id = Guid.NewGuid(),
                                ProductType = "Mua hộ",
                                ProductTopicId = topic.Id,
                                ProductTopic = topic.Code,
                                Code = "golf-" + item.ProductCode + "-" + topic.Code,
                                Name = item.ProductName,
                                SourceLink = item.ProductLink,
                                SourceCode = item.ProductCode,
                                Image = item.PreviewImage,
                                //Images = string.Join(",", item.Images.Any() ? item.Images:""),
                                Price = item.Price,
                                Tax = item.Tax,
                                Currency = item.Currency,
                                Status = 2,
                                CreatedDate = DateTime.Now,
                                Exp = DateTime.Now.AddDays(4),
                                Channel = "GOLF"
                            };
                            listUpdate.Add(i);
                            //await store.SP_CREATE_PRODUCT_TOPIC_DETAILAsync(i.Id, i.ProductType, i.ProductTopicId, i.ProductTopic,
                            // i.Code, i.Condition, i.Unit, i.Name, i.SourceLink, i.SourceCode, i.ShortDescription,
                            // i.FullDescription, i.Origin, i.Brand, i.Manufacturer, i.Image, i.Images,
                            // i.Price, i.Currency, i.Status, i.Tags, i.Exp, i.BidPrice,
                            //i.Tax, i.Channel, i.CreatedBy, i.CreatedByName);

                        }
                }
            }
            if (query.SourceCode.ToLower().Equals("rak"))
            {
                var pageQuery = 1;
                if (query.PageQuery.HasValue) pageQuery = query.PageQuery.Value;
                for (var p = 1; p <= pageQuery; p++)
                {
                    Thread.Sleep(rnd.Next(1000, 5000));
                    var searchData = await spider.RakutenSearch(query.Keyword, query.Category, "", query.Seller, p);
                    var pItems = searchData.Items;
                    if (pItems != null)
                        foreach (var item in pItems)
                        {
                            if (item.ProductLink.Contains("ias.rakuten.co.jp")) continue;
                            var i = new ProductTopicDetail()
                            {
                                Id = Guid.NewGuid(),
                                ProductType = "Mua hộ",
                                ProductTopicId = topic.Id,
                                ProductTopic = topic.Code,
                                Code = "rak-" + item.ProductCode + "-" + topic.Code,
                                Name = item.ProductName,
                                SourceLink = item.ProductLink,
                                SourceCode = item.ProductCode,
                                Image = item.PreviewImage,
                                //Images = string.Join(",", item.Images.Any() ? item.Images:""),
                                Price = item.Price,
                                Tax = item.Tax,
                                Currency = item.Currency,
                                Status = 2,
                                CreatedDate = DateTime.Now,
                                Exp = DateTime.Now.AddDays(4),
                                Channel = "RAK"
                            };
                            listUpdate.Add(i); 

                        }
                }
            }
        }

        foreach (var i in listUpdate.OrderBy(x => Guid.NewGuid()))
        {
            Thread.Sleep(50);
            await store.SP_CREATE_PRODUCT_TOPIC_DETAILAsync(i.Id, i.ProductType, i.ProductTopicId, i.ProductTopic,
                        i.Code, i.Condition, i.Unit, i.Name, i.SourceLink, i.SourceCode, i.ShortDescription,
                        i.FullDescription, i.Origin, i.Brand, i.Manufacturer, i.Image, i.Images,
                        i.Price, i.Currency, i.Status, i.Tags, i.Exp, i.BidPrice,
                       i.Tax, i.Channel, i.CreatedBy, i.CreatedByName);
        }
        //Console.WriteLine(test);



    }
}


