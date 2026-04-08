using Core.Entities;
using Core.Interfaces;
using Core.Repositories;
using Infrastructure.Cosmos.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Cosmos.Extensions
{
    public static class StartupExtension
    {
        public const string DEFAULT_CAR_CONTAINER = "Cars";
        public const string DEFAULT_RENTALS_CONTAINER = "Rentals";
        public const string DEFAULT_COSMOS_CONFIG = "CosmosDbSettings";
        public const string COSMOS_CONFIG_ENDPOINT = "AccountEndpoint";
        public const string COSMOS_CONFIG_AUTHKEY = "AuthKey";
        public const string COSMOS_CONFIG_DB_NAME = "DatabaseName";

        public static IServiceCollection AddCosmosServices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection(DEFAULT_COSMOS_CONFIG) ?? throw new ArgumentNullException();

            string accountEndpoint = config[COSMOS_CONFIG_ENDPOINT] ?? throw new ArgumentNullException();
            string accountKey = config[COSMOS_CONFIG_AUTHKEY] ?? throw new ArgumentNullException();
            string databaseName = config[COSMOS_CONFIG_DB_NAME] ?? throw new ArgumentNullException();

            // Register CosmosDb Context
            services.AddDbContext<CarRentalContext>(options =>
                options.UseCosmos(accountEndpoint, accountKey, databaseName));

            services.AddScoped<EfRepository<CarEntity, CarRentalContext>>();
            services.AddScoped<IRepository<CarEntity, Guid>, EfRepository<CarEntity, CarRentalContext>>();

            services.AddScoped<EfRepository<RentalEntity, CarRentalContext>>();
            services.AddScoped<IRepository<RentalEntity, Guid>, EfRepository<RentalEntity, CarRentalContext>>();

            return services;
        }
    }
}
