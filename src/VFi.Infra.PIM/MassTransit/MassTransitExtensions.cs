using Consul; 
using VFi.Infra.PIM.MassTransit.Consumers;
using VFi.Infra.PIM.Repository;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VFi.NetDevPack.Configuration;
using RabbitMQ.Client;
using System;
using System.Data;
using System.Linq;
using VFi.Infra.ACC.MassTransit.Consumers;
using VFi.Domain.PIM.Events;

namespace VFi.Infra.PIM.Consul
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {

            var queueSettings = configuration.GetSection("RabbitConfig").Get<RabbitConfig>();
            //services.AddSingleton(sp => configuration.GetSection("RabbitConfig").Get<RabbitConfig>());
            if (queueSettings != null)
                if (queueSettings.RabbitEnabled)
                {
                    services.AddMassTransit(x =>
                    {
                        if (queueSettings.ConsumerEnabled)
                        {
                            x.AddConsumer<AddProductCrossConsumer>();
                            x.AddConsumer<AddProductTopicConsumer>();
                            x.AddConsumer<PublishProductTopicItemConsumer>();
                        }
                        


                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(queueSettings.RabbitHostName, queueSettings.RabbitVirtualHost, h => {
                                h.Username(queueSettings.RabbitUsername);
                                h.Password(queueSettings.RabbitPassword);
                            });
                            //cfg.ConfigureEndpoints(context);

                            if (queueSettings.ConsumerEnabled)
                            {
                                cfg.ReceiveEndpoint("add-product-cross-event", e =>
                                {
                                    e.ConfigureConsumer<AddProductCrossConsumer>(context);
                                    e.PrefetchCount = 1;
                                });
                                cfg.ReceiveEndpoint("add-product-topic-event", e =>
                                {
                                    e.ConfigureConsumer<AddProductTopicConsumer>(context);
                                    e.PrefetchCount = 2;
                                });
                                cfg.ReceiveEndpoint("publish-product-topic-item-event", e =>
                                {
                                    e.ConfigureConsumer<PublishProductTopicItemConsumer>(context);
                                    e.PrefetchCount = 2;
                                });
                            }

                        });
                    });

                    if (queueSettings.PublisherEnabled)
                    {
                       // EndpointConvention.Map<UserEmailEvent>(queueSettings.BuildEndPoint("user-email-event")); 
                       // services.AddScoped<IEmailRepository, EmailRepository>();
                    }

                }

            //var provider = services.BuildServiceProvider();
            //var busControl = provider.GetRequiredService<IBusControl>();



            return services;
        }

        public static IApplicationBuilder UseRabbitMQ(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
        {

             
            return app;
        }
    }
}
