using Core.Interfaces;
using Core.Pricing;
using Core.Pricing.Abstractions;
using Core.Pricing.InMemoryProviders;
using Core.Services;
using Core.Strategies;
using Infrastructure.Cosmos.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IPriceProvider, InMemoryPriceProvider>();

builder.Services.AddScoped<IPriceStrategy, SubcompactPriceStrategy>();
builder.Services.AddScoped<IPriceStrategy, StationWagonPriceStrategy>();
builder.Services.AddScoped<IPriceStrategy, TruckPriceStrategy>();

builder.Services.AddSingleton<PriceStrategyResolver>();
builder.Services.AddScoped<PriceCalculator>();

builder.Services.AddScoped<ICarsService, CarsService>();
builder.Services.AddScoped<IRentalsService, RentalsService>();

#region Cosmos implementation
builder.Services.AddCosmosServices(builder.Configuration);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
