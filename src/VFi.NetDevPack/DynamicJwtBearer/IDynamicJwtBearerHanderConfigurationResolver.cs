﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading.Tasks;

namespace VFi.NetDevPack.DynamicJwtBearer
{
    public interface IDynamicJwtBearerHanderConfigurationResolver
    {
        Task<OpenIdConnectConfiguration> ResolveCurrentOpenIdConfiguration(HttpContext context);
    }
}
