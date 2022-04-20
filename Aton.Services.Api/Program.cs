using System.Reflection;
using Aton.Infrastructure.Data.Contexts;
using Aton.Infrastructure.IoC;
using Aton.Services.Api.Configurations;
using Aton.Services.Api.StartupExtensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
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