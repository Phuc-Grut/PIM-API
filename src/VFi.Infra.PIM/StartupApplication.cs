using VFi.Infra.PIM.Context;
using VFi.Infra.PIM.EventSourcing;
using VFi.Infra.PIM.Repository.EventSourcing;
using VFi.NetDevPack.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VFi.NetDevPack;
using VFi.NetDevPack.Mediator;
using VFi.Infra.PIM.Consul;
using VFi.NetDevPack.Context;
using VFi.Infra.PIM.Repository;
using VFi.Domain.PIM.Interfaces;
using VFi.NetDevPack.Configuration;
using VFi.NetDevPack.Data;
using Consul;
using Microsoft.Extensions.Options;
using Flurl.Http.Configuration;
using MassTransit.JobService;
using PuppeteerSharp;
using VFi.Infra.PIM.Spider.Algorithm;
using VFi.Infra.PIM.Spider.Interface;
using VFi.Infra.PIM.Spider;

namespace VFi.Infra.PIM
{
    public  class StartupApplication: IStartupApplication
    {
        public int Priority => 112;
        public bool BeforeConfigure => true;

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var env = serviceProvider.GetService<IHostingEnvironment>();


            services.AddHttpClient();
            services.AddConsul(configuration);
            //services.AddDbContext<SqlCoreContext>(options =>options.UseSqlServer(configuration.GetConnectionString("CoreConnection")));
            var connectionStringPlaceHolder = configuration.GetConnectionString("PIMConnection");
            services.AddDbContext<SqlCoreContext>((serviceProvider, dbContextBuilder) =>
            {
                var context = serviceProvider.GetRequiredService<IContextUser>();
                var connectionString = connectionStringPlaceHolder.Replace("{data_zone}", context.Data_Zone).Replace("{data}", context.Data);
                dbContextBuilder.UseSqlServer(connectionString);
            });

            //services.AddDbContext<EventStoreSqlContext>(options => options.UseSqlServer(configuration.GetConnectionString("EventConnection"))); 
            var connectionEventPlaceHolder = configuration.GetConnectionString("PIMEventConnection");
            services.AddDbContext<EventStoreSqlContext>((serviceProvider, dbContextBuilder) =>
            {
                var context = serviceProvider.GetRequiredService<IContextUser>();
                var connectionEvent = connectionEventPlaceHolder.Replace("{data_zone}", context.Data_Zone).Replace("{data}", context.Data);
                dbContextBuilder.UseSqlServer(connectionEvent);
            });

            services.AddTransient<Publisher>();
            services.AddTransient<IMediatorHandler, VFi.Infra.PIM.Bus.MediatorHandler>();
            services.AddScoped<EventStoreSqlContext>();
            services.AddTransient<IEventStore, SqlEventStore>();
            services.AddTransient<IEventStoreRepository, EventStoreSqlRepository>();


            // Infra - Data

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IGroupCategoryRepository, GroupCategoryRepository>();
            services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
            services.AddScoped<IProductBrandRepository, ProductBrandRepository>();
            services.AddScoped<IProductOriginRepository, ProductOriginRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IDeliveryTimeRepository, DeliveryTimeRepository>();
            services.AddScoped<IGroupUnitRepository, GroupUnitRepository>(); 
            services.AddScoped<IProductTagRepository, ProductTagRepository>();
            services.AddScoped<IStoreRepository, StoreRepository>();
            services.AddScoped<ITaxCategoryRepository, TaxCategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductCategoryMappingRepository, ProductCategoryMappingRepository>();
            services.AddScoped<IProductProductTagMappingRepository, ProductProductTagMappingRepository>();
            services.AddScoped<IProductSpecificationAttributeMappingRepository, ProductSpecificationAttributeMappingRepository>();
            services.AddScoped<IProductVariantAttributeCombinationRepository, ProductVariantAttributeCombinationRepository>();
            services.AddScoped<IProductVariantAttributeValueRepository, ProductVariantAttributeValueRepository>();
            services.AddScoped<IProductInventoryRepository, ProductInventoryRepository>();
            services.AddScoped<IProductPackageRepository, ProductPackageRepository>();
            services.AddScoped<IProductMediaRepository, ProductMediaRepository>();
            services.AddScoped<IProductReviewHelpfulnessRepository, ProductReviewHelpfulnessRepository>();
            services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
            services.AddScoped<ITierPriceRepository, TierPriceRepository>();
            services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
            services.AddScoped<IProductAttributeOptionRepository, ProductAttributeOptionRepository>(); 
            services.AddScoped<ISpecificationAttributeRepository, SpecificationAttributeRepository>();
            services.AddScoped<ISpecificationAttributeOptionRepository, SpecificationAttributeOptionRepository>();
            services.AddScoped<IServiceAddRepository, ServiceAddRepository>();
            services.AddScoped<IServiceAddPriceSyntaxRepository, ServiceAddPriceSyntaxRepository>();
            services.AddScoped<IProductStoreMappingRepository, ProductStoreMappingRepository>();
            services.AddScoped<IProductGroupCategoryMappingRepository, ProductGroupCategoryMappingRepository>();
            services.AddScoped<IProductProductAttributeMappingRepository, ProductProductAttributeMappingRepository>();
            services.AddScoped<IRelatedProductRepository, RelatedProductRepository>();
            services.AddScoped<IProductServiceAddRepository, ProductServiceAddRepository>();
            services.AddScoped<IProductWarehouseRepository, ProductWarehouseRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<ICategoryRootRepository, CategoryRootRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<ISyntaxCodeRepository, SyntaxCodeRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IProductSpecificationCodeRepository, ProductSpecificationCodeRepository>();
            services.AddScoped<IProductTopicQueryRepository, ProductTopicQueryRepository>();

            services.AddScoped<IProductTopicRepository, ProductTopicRepository>();
            services.AddScoped<IProductTopicPageRepository, ProductTopicPageRepository>();
            services.AddScoped<IProductTopicPageMapRepository, ProductTopicPageMapRepository>();
            services.AddScoped<IProductTopicDetailRepository, ProductTopicDetailRepository>();
            services.AddScoped<SqlCoreContext>();
            services.AddScoped<IPIMContextProcedures, PIMContextProcedures>();                      
            services.AddRabbitMQ(configuration);
            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "redis.local:6379";
            });


            services.AddSingleton<IMasterRepository, MasterRepository>();
            services.AddSingleton<IFlurlClientFactory, PerBaseUrlFlurlClientFactory>();
            services.AddSingleton<MasterApiContext>();

            services.AddScoped<ISpiderRepository, SpiderRepository>();
            services.AddScoped<IMercariRepository, MercariRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();

            services.AddSingleton<SpiderApiContext>();
            services.AddSingleton<ChooseRequestAlgorithm, ChooseRequestRandomAlgorithm>();
            services.AddSingleton<ILoadProxy, LoadProxy>();
            services.AddSingleton<ICreateHttpClient, CreateHttpClient>();
            services.AddSingleton<IRequestManager, RequestManager>();
            services.AddSingleton<MercariToken>();
        }

        public void Configure(WebApplication application, IWebHostEnvironment webHostEnvironment)
        {
            try
            {
                application.UseConsul(application.Lifetime);
                application.Lifetime.ApplicationStarted.Register(() =>
                {
                    try
                    {
                        new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
                    }
                    catch (Exception) { }
                });
            }
            catch (Exception)
            {
            }
        }
    }
}