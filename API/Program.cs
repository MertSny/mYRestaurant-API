using API.Utily;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.DependencyResolvers.Autofac;
using Business.Mapper;
using Core.DependencyResolvers;
using Core.Extension;
using Core.Utulities.IOC;
using Core.Utulities.Security.Encryption;
using Core.Utulities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder => { builder.RegisterModule(new AutofacBusinessModule()); });

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDbContext<DatabaseContext>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.Converters.Add(new EmptyStringToNullJsonConverter());
});

builder.Services.AddAuthorization();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = int.MaxValue;
});

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MultipartBoundaryLengthLimit = int.MaxValue;
    o.MultipartHeadersCountLimit = int.MaxValue;
    o.MultipartHeadersLengthLimit = int.MaxValue;
    o.BufferBodyLengthLimit = int.MaxValue;
    o.BufferBody = true;
    o.ValueCountLimit = int.MaxValue;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCourse API", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer sheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                    {
                            Reference=new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme="oauth2",
                            Name="Bearer",
                            In=ParameterLocation.Header,
                    } ,
                        new List<string>()
                    }
                });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:3001", "http://localhost:4104", "http://10.68.216.54:4104", "https://myexpenses.msctr.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var userDal = context.HttpContext.RequestServices.GetRequiredService<IUserDal>();
                var userId = Convert.ToInt32(context.Principal.Claims.FirstOrDefault(u => u.Type == "systemUserId").Value);
                var user = await userDal.Count(p => p.Id == userId);
                if (user == 0)
                {
                    context.Fail("Unauthorized");
                }
            }
        };
        options.SaveToken = true;
    });

builder.Services.AddDependencyResolvers(new ICoreModule[] {
                new CoreModule(),
});


var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    try
//    {
//        var context = services.GetRequiredService<DatabaseContext>();
//        if (context.Database.IsSqlServer())
//            context.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

//        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

//        throw;
//    }
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyCourse API Version v1");
        c.DocExpansion(DocExpansion.None);
        c.InjectStylesheet("/css/custom.css");
        c.InjectJavascript("/js/custom.js");
        c.DocumentTitle = "MyCourse API";
    });
}

app.Use(async (context, next) =>
{
    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null; // unlimited I guess
    await next.Invoke();
});

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.ConfigureCustomExceptionMiddleware();

app.UseStaticFiles();

app.UseCors(builder => builder.WithOrigins("http://localhost:3001", "http://localhost:4104", "http://10.68.216.54:4104", "https://myexpenses.msctr.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

var requestOpt = new RequestLocalizationOptions();
requestOpt.SupportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en-US")
            };
requestOpt.SupportedUICultures = new List<CultureInfo>
            {
                new CultureInfo("en-US")
            };
requestOpt.RequestCultureProviders.Clear();
requestOpt.RequestCultureProviders.Add(new SingleCultureProvider());

app.MapControllers();

app.Run();
