using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace StocksApp.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();

            services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));
            services.AddScoped<IFinnhubGetterService, FinnhubGetterService>();
            services.AddScoped<IBuyOrderCreatorService, BuyOrderCreatorService>();
            services.AddScoped<IBuyOrderGetterService, BuyOrderGetterService>();
            services.AddScoped<ISellOrderCreatorService, SellOrderCreatorService>();
            services.AddScoped<ISellOrderGetterService, SellOrderGetterService>();
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IStocksRepository, StocksRepository>();

            return services;
        }
    }
}
