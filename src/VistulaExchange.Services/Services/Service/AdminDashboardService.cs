using VistulaExchange.Services.Models;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IUserService _userService;
        private readonly IAvailableMoneyOnStockService _availableMoneyOnStockService;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;

        public AdminDashboardService(
            ITransactionHistoryService transactionHistoryService,
            IUserService userService,
            IAvailableMoneyOnStockService availableMoneyOnStockService,
            IExchangeOfficeBoardService exchangeOfficeBoardService)
        {
            _transactionHistoryService = transactionHistoryService;
            _userService = userService;
            _availableMoneyOnStockService = availableMoneyOnStockService;
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
        }

        public async Task<AdminDashboardDto> GetDashboardAsync()
        {
            var histories = await _transactionHistoryService.GetAllUsersHistoriesAsync();
            var liquidity = await _availableMoneyOnStockService.GetAvailableMoneyOnStockAsync();
            var currencies = _exchangeOfficeBoardService.GetAllCurrencies()
                .Where(currency => !string.Equals(currency.ShortName, "PLN", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var now = DateTime.Now;
            var sixMonthsAgo = new DateTime(now.Year, now.Month, 1).AddMonths(-5);

            var monthlyActivity = Enumerable.Range(0, 6)
                .Select(offset => sixMonthsAgo.AddMonths(offset))
                .Select(month => new AdminTimelinePointDto
                {
                    Label = month.ToString("MMM"),
                    Value = histories.Count(item => item.DateOfTransaction.Year == month.Year && item.DateOfTransaction.Month == month.Month)
                })
                .ToList();

            var turnover = histories
                .GroupBy(history => history.CurrencyName)
                .Select(group => new AdminCurrencyTurnoverDto
                {
                    CurrencyCode = group.Key,
                    Amount = Math.Round(group.Sum(item => item.Amount), 2)
                })
                .OrderByDescending(item => item.Amount)
                .Take(6)
                .ToList();

            var hottestCurrency = histories
                .GroupBy(history => history.CurrencyName)
                .Select(group => new { CurrencyCode = group.Key, Count = group.Count() })
                .OrderByDescending(group => group.Count)
                .FirstOrDefault();

            return new AdminDashboardDto
            {
                UserAccounts = await _userService.GetUsersCountAsync(),
                UserEmailConfirmed = await _userService.GetConfirmedUsersCountAsync(),
                DailyTransactions = histories.Count(item => item.DateOfTransaction.Date == now.Date),
                MonthlyTransactions = histories.Count(item => item.DateOfTransaction.Year == now.Year && item.DateOfTransaction.Month == now.Month),
                AnnualTransactions = histories.Count(item => item.DateOfTransaction.Year == now.Year),
                AverageSpread = currencies.Count == 0 ? 0 : Math.Round(currencies.Average(currency => currency.SellAt - currency.BuyAt), 4),
                HottestCurrencyCode = hottestCurrency?.CurrencyCode ?? string.Empty,
                HottestCurrencyTransactions = hottestCurrency?.Count ?? 0,
                Turnovers = turnover,
                Liquidity = liquidity
                    .OrderByDescending(item => item.Value)
                    .Select(item => new AdminLiquidityDto
                    {
                        CurrencyCode = item.CurrencyName,
                        Value = Math.Round(item.Value, 2)
                    })
                    .ToList(),
                MonthlyActivity = monthlyActivity,
                LatestTransactions = histories
                    .OrderByDescending(item => item.DateOfTransaction)
                    .Take(8)
                    .Select(item => new UserDashboardTransactionDto
                    {
                        CurrencyCode = item.CurrencyName,
                        Amount = item.Amount,
                        DateOfTransaction = item.DateOfTransaction,
                        Description = item.Description,
                        UserDisplayName = item.User is null
                            ? "Client"
                            : string.Join(" ", new[] { item.User.FirstName, item.User.LastName }.Where(value => !string.IsNullOrWhiteSpace(value))).Trim(),
                        UserEmail = item.User?.Email ?? string.Empty
                    })
                    .ToList()
            };
        }
    }
}
