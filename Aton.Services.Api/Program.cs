using System.Reflection;
using Aton.Infrastructure.IoC;
using Aton.Services.Api.Configurations;
using Aton.Services.Api.Filters;
using Aton.Services.Api.Services;
using Aton.Services.Api.StartupExtensions;
using MediatR;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter {AllowIntegerValues = true});
        options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
    });
    // .AddJsonOptions(options =>
    // {
    //     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: true));
    //     options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    // });
services.AddEndpointsApiExplorer();
#region Configure Swagger  
services.AddSwaggerGen(c =>  
{  
    c.EnableAnnotations();
    c.SchemaFilter<SwaggerIgnoreFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Aton", Version = "v1" });  
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme  
    {  
        Name = "Authorization",  
        Type = SecuritySchemeType.Http,  
        Scheme = "basic",  
        In = ParameterLocation.Header,  
        Description = "Basic Authorization header using the Bearer scheme."  
    });  
    c.AddSecurityRequirement(new OpenApiSecurityRequirement  
    {  
        {  
            new OpenApiSecurityScheme  
            {  
                Reference = new OpenApiReference  
                {  
                    Type = ReferenceType.SecurityScheme,  
                    Id = "basic"  
                }  
            },  
            new string[] {}  
        }  
    });  
}); 
services.AddSwaggerGenNewtonsoftSupport();
#endregion  
// ----- Database -----
services.AddCustomizedDatabase(builder.Configuration, builder.Environment);

// ----- Auth -----
services.AddCustomizedAuth(builder.Configuration);

// ----- Http -----
//services.AddCustomizedHttp(builder.Configuration);

// ----- AutoMapper -----
services.AddAutoMapperSetup();

// Adding MediatR for Domain Events and Notifications
services.AddMediatR(Assembly.GetExecutingAssembly());

services.AddCustomizedHash(builder.Configuration);

services.AddScoped<IUserAccountConnector, UserAccountConnector>();


// ----- Health check -----
//services.AddCustomizedHealthCheck(builder.Configuration, builder.Environment);

// .NET Native DI Abstraction

NativeInjectorBootStrapper.RegisterServices(services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomizedErrorHandling(app.Environment);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ----- CORS -----
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

namespace Aton.Services.Api
{
    public partial class Program { }
}