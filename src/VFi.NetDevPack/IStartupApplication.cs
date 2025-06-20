﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack
{
    public interface IStartupApplication
    {
        

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Gets before configure of this startup configuration implementation
        /// </summary>
        bool BeforeConfigure { get; }


        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        /// <param name="webHostEnvironment">WebHostEnvironment</param>
        void Configure(WebApplication application, Microsoft.AspNetCore.Hosting.IWebHostEnvironment webHostEnvironment);


        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration root of the application</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
