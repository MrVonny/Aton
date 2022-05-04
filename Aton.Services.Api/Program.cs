using System.Reflection;
using Aton.Infrastructure.IoC;
using Aton.Services.Api.Configurations;
using Aton.Services.Api.Services;
using Aton.Services.Api.StartupExtensions;
using MediatR;
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

// ----- Swagger -----
services.AddCustomizedSwagger(builder.Environment);

// ----- Database -----
services.AddCustomizedDatabase(builder.Configuration, builder.Environment);

// ----- Auth -----
services.AddCustomizedAuth(builder.Configuration);

// ----- AutoMapper -----
services.AddAutoMapperSetup();

// Adding MediatR for Domain Events and Notifications
services.AddMediatR(Assembly.GetExecutingAssembly());

services.AddCustomizedHash(builder.Configuration);

services.AddScoped<IUserAccountConnector, UserAccountConnector>();


// .NET Native DI Abstraction

NativeInjectorBootStrapper.RegisterServices(services);

var app = builder.Build();

app.UseCustomizedSwagger(app.Environment);
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