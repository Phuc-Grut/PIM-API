
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.Provider.Polly;
using MMLib.Ocelot.Provider.AppConfiguration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using VFi.Gateway.PIM.Handlers;
using VFi.NetDevPack.Context;
using System.Security.Claims;
using VFi.NetDevPack.DynamicJwtBearer.DynamicAuthority;
using Ocelot.Values;
using VFi.NetDevPack.DynamicJwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;
using Polly;
using static IdentityModel.OidcConstants;
using Microsoft.AspNetCore.Builder;

var ocelotJson = new JObject();
var builder = WebApplication.CreateBuilder(args);
if (!builder.Environment.IsDevelopment())
{
    foreach (var jsonFilename in Directory.EnumerateFiles("Configuration", "ocelot.*.Production.json", SearchOption.AllDirectories))
    {
        using (StreamReader fi = File.OpenText(jsonFilename))
        {
            var json = JObject.Parse(fi.ReadToEnd());
            ocelotJson.Merge(json, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
        }
    }
}
else
{
    foreach (var jsonFilename in Directory.EnumerateFiles("Configuration", "ocelot.*.Development.json", SearchOption.AllDirectories))
    {
        using (StreamReader fi = File.OpenText(jsonFilename))
        {
            var json = JObject.Parse(fi.ReadToEnd());
            ocelotJson.Merge(json, new JsonMergeSettings
            {
                MergeArrayHandling = MergeArrayHandling.Union
            });
        }
    }
}


File.WriteAllText("ocelot.json", ocelotJson.ToString());
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json",
                    optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",
                    optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddAspNetUserConfiguration();
builder.Services
                .AddSwaggerForOcelot(builder.Configuration)
                .AddOcelot(builder.Configuration)
                .AddAppConfiguration().AddPolly()
                .AddConsul()
                .AddConfigStoredInConsul()
                .AddDelegatingHandler<VFi.Gateway.PIM.Handlers.TokenHandler>(false)
                .AddDelegatingHandler<VFi.Gateway.PIM.Handlers.AuthInternalHandler>(false);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Gateway", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });

});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
var audienceConfig = builder.Configuration.GetSection("Audience");
 

builder.Services.AddMemoryCache();
builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddDynamicJwtBearer("OidcSecurity", options =>
                {
                    options.TokenValidationParameters.ValidateAudience = false;
                })
                .AddDynamicAuthorityJwtBearerResolver<ResolveAuthorityService>();
 

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.UseSwaggerForOcelotUI(opt =>
{
    opt.RoutePrefix = "docs";
    opt.DownstreamSwaggerHeaders = new[]
                    {
                        new KeyValuePair<string, string>("Key", "Value"),
                        new KeyValuePair<string, string>("Key2", "Value2"),
                    };
    opt.PathToSwaggerGenerator = "/swagger/docs";
});
app.UseCors("CorsPolicy");
app.UseOcelot().Wait();
 

app.Run();
public static class TokenLifetimeValidator
{
    public static bool Validate(
        DateTime? notBefore,
        DateTime? expires,
        SecurityToken tokenToValidate,
        TokenValidationParameters @param
    )
    {
        var temp = expires > DateTime.UtcNow;
        return (expires != null && expires > DateTime.UtcNow);
    }
}


internal class ResolveAuthorityService : IDynamicJwtBearerAuthorityResolver
{
    private readonly IConfiguration configuration;

    public ResolveAuthorityService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public TimeSpan ExpirationOfConfiguration => TimeSpan.FromHours(1);

    public Task<string> ResolveAuthority(HttpContext httpContext)
    {
        var audienceConfig = configuration.GetSection("Audience");
        string authorization = httpContext.Request.Headers[HeaderNames.Authorization];
        var token = "";
        if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = authorization.Substring("Bearer ".Length).Trim();
        }
        var jwt_token = new JwtSecurityToken(token);
        var realm = jwt_token.Claims.First(c => c.Type == "tenant").Value;
        var authority = $"{audienceConfig["Iss"]}/realms/{realm}"; 
        return Task.FromResult(authority);
    }
}