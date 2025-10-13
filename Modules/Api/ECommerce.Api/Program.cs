using ECommerce.Api.Extensions;
using ECommerce.Orders;
using ECommerce.Shared.Core.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerModule();

/* Modules */
builder.Services.AddOrdersModule(builder.Configuration);

var app = builder.Build();

app.UseSwaggerModule();

app.UseMapEndpoints();

app.Run();