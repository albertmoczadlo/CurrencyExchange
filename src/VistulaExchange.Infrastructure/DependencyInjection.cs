using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;
using VistulaExchange.Infrastructure.Repositories;

namespace VistulaExchange.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("VistulaExchangeDbContextConnection")
                ?? throw new InvalidOperationException("Connection string 'VistulaExchangeDbContextConnection' not found.");

            services.AddDbContext<VistulaExchangeDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddTransient<IExchangeOfficeBoardRepository, ExchangeOfficeBoardRepositoryDB>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITransactionHistoryRepository, TransactionHistoryRepositoryDB>();
            services.AddTransient<IExchangeOfficeRepository, ExchangeOfficeRepositoryDB>();
            services.AddTransient<IUserWalletRepository, UserWalletRepository>();
            services.AddTransient<IAvailableMoneyOnStockRepository, AvailableMoneyOnStockRepository>();
            services.AddTransient<IUserAlarmsRepository, UserAlarmRepository>();

            return services;
        }
    }
}
