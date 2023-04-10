using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
				//options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
				options.UseMySql(configuration["ConnectionStrings:AZURE_MYSQL_CONNECTIONSTRING"],
							ServerVersion.AutoDetect(
							  configuration["ConnectionStrings:AZURE_MYSQL_CONNECTIONSTRING"]
							  )
							);

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
