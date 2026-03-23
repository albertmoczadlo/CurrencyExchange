using Microsoft.Extensions.DependencyInjection;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Services.Services.Service;

namespace VistulaExchange.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IExchangeOfficeBoardService, ExchangeOfficeBoardService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INbpJsonService, NbpJsonService>();
            services.AddTransient<ITransactionHistoryService, TransactionHistoryService>();
            services.AddTransient<IExchangeOfficeService, ExchangeOfficeService>();
            services.AddTransient<IUserWalletService, UserWalletService>();
            services.AddTransient<IUserAlarmsService, UserAlarmsService>();
            services.AddTransient<IAvailableMoneyOnStockService, AvailableMoneyOnStockService>();
            services.AddTransient<IUserDashboardService, UserDashboardService>();
            services.AddTransient<IAdminDashboardService, AdminDashboardService>();
            services.AddTransient<ITradeQuoteService, TradeQuoteService>();

            return services;
        }
    }
}
