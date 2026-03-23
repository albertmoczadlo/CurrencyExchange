using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Models;
using VistulaExchange.Services.Models;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IUserWalletService _userWalletService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IUserAlarmsService _userAlarmsService;
        private readonly IUserService _userService;
        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;
        private readonly INbpJsonService _nbpJsonService;

        public UserDashboardService(
            IUserWalletService userWalletService,
            ITransactionHistoryService transactionHistoryService,
            IUserAlarmsService userAlarmsService,
            IUserService userService,
            IExchangeOfficeBoardService exchangeOfficeBoardService,
            INbpJsonService nbpJsonService)
        {
            _userWalletService = userWalletService;
            _transactionHistoryService = transactionHistoryService;
            _userAlarmsService = userAlarmsService;
            _userService = userService;
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
            _nbpJsonService = nbpJsonService;
        }

        public async Task<UserDashboardDto> GetDashboardAsync(string userId)
        {
            var userTask = _userService.GetUserByIdAsync(userId);
            var walletTask = _userWalletService.GetWalletAsync(userId);
            var transactionTask = _transactionHistoryService.GetUserHistoryByUserIdAsync(userId);
            var alarmTask = _userAlarmsService.GetUserAlarmsAsync(userId);

            await Task.WhenAll(userTask, walletTask, transactionTask, alarmTask);

            var user = userTask.Result;
            var wallet = walletTask.Result;
            var transactions = transactionTask.Result
                .OrderByDescending(item => item.DateOfTransaction)
                .ToList();
            var alarms = alarmTask.Result;

            var boardCurrencies = _exchangeOfficeBoardService.GetAllCurrencies()
                .ToDictionary(currency => currency.ShortName, StringComparer.OrdinalIgnoreCase);

            var positions = await BuildPositionsAsync(wallet, boardCurrencies);
            var alarmDtos = BuildAlarmDtos(alarms, boardCurrencies);
            var insights = BuildInsights(positions, alarmDtos, transactions);

            var previousValue = positions.Sum(position => position.ValueInPln - position.DailyChangeValue);
            var dailyChangeValue = positions.Sum(position => position.DailyChangeValue);
            var dailyChangePercent = previousValue <= 0
                ? 0
                : Math.Round((dailyChangeValue / previousValue) * 100m, 2);

            return new UserDashboardDto
            {
                UserDisplayName = string.Join(" ", new[] { user.FirstName, user.LastName }.Where(value => !string.IsNullOrWhiteSpace(value))).Trim(),
                TotalValueInPln = Math.Round(positions.Sum(position => position.ValueInPln), 2),
                DailyChangeValue = Math.Round(dailyChangeValue, 2),
                DailyChangePercent = dailyChangePercent,
                DefaultChartCurrencyCode = positions.FirstOrDefault(position => !string.Equals(position.CurrencyCode, "PLN", StringComparison.OrdinalIgnoreCase))?.CurrencyCode
                    ?? boardCurrencies.Keys.FirstOrDefault(code => !string.Equals(code, "PLN", StringComparison.OrdinalIgnoreCase))
                    ?? "EUR",
                Positions = positions,
                RecentTransactions = transactions
                    .Take(6)
                    .Select(item => new UserDashboardTransactionDto
                    {
                        CurrencyCode = item.CurrencyName,
                        Amount = item.Amount,
                        DateOfTransaction = item.DateOfTransaction,
                        Description = item.Description,
                        UserDisplayName = string.Join(" ", new[] { item.User?.FirstName, item.User?.LastName }.Where(value => !string.IsNullOrWhiteSpace(value))).Trim(),
                        UserEmail = item.User?.Email ?? user.Email ?? string.Empty
                    })
                    .ToList(),
                ActiveAlarms = alarmDtos,
                Insights = insights,
                MarketQuotes = boardCurrencies.Values
                    .Where(currency => !string.Equals(currency.ShortName, "PLN", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(currency => currency.ShortName)
                    .Select(currency => new MarketQuoteDto
                    {
                        Id = currency.Id,
                        CurrencyCode = currency.ShortName,
                        CurrencyName = currency.Name,
                        Country = currency.Country,
                        BuyAt = currency.BuyAt,
                        SellAt = currency.SellAt
                    })
                    .ToList()
            };
        }

        private async Task<IReadOnlyList<UserDashboardPositionDto>> BuildPositionsAsync(
            Wallet wallet,
            IReadOnlyDictionary<string, Currency> boardCurrencies)
        {
            var walletPositions = wallet.WalletPositions?
                .Where(position => position.Currency is not null)
                .OrderBy(position => position.Currency.ShortName)
                .ToList() ?? new List<WalletPosition>();

            var historyTasks = walletPositions
                .Where(position => !string.Equals(position.Currency.ShortName, "PLN", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(
                    position => position.Currency.ShortName,
                    position => _nbpJsonService.GetCurrencyHistoryAsync(position.Currency.ShortName, 2),
                    StringComparer.OrdinalIgnoreCase);

            await Task.WhenAll(historyTasks.Values);

            return walletPositions
                .Select(position =>
                {
                    var code = position.Currency.ShortName;
                    var liveCurrency = boardCurrencies.TryGetValue(code, out var currentCurrency)
                        ? currentCurrency
                        : position.Currency;
                    var history = historyTasks.TryGetValue(code, out var historyTask)
                        ? historyTask.Result
                        : Array.Empty<CurrencyHistoryPoint>();
                    var previousPoint = history.OrderByDescending(item => item.Date).Skip(1).FirstOrDefault()
                        ?? history.OrderBy(item => item.Date).FirstOrDefault();
                    var currentSell = string.Equals(code, "PLN", StringComparison.OrdinalIgnoreCase) ? 1m : liveCurrency.SellAt;
                    var currentBuy = string.Equals(code, "PLN", StringComparison.OrdinalIgnoreCase) ? 1m : liveCurrency.BuyAt;
                    var previousSell = previousPoint?.SellAt ?? currentSell;
                    var valueInPln = string.Equals(code, "PLN", StringComparison.OrdinalIgnoreCase)
                        ? position.CurrencyAmount
                        : position.CurrencyAmount * currentSell;
                    var dailyChangeValue = string.Equals(code, "PLN", StringComparison.OrdinalIgnoreCase)
                        ? 0m
                        : (currentSell - previousSell) * position.CurrencyAmount;
                    var previousValue = valueInPln - dailyChangeValue;
                    var dailyChangePercent = previousValue <= 0
                        ? 0
                        : Math.Round((dailyChangeValue / previousValue) * 100m, 2);

                    return new UserDashboardPositionDto
                    {
                        CurrencyCode = code,
                        CurrencyName = liveCurrency.Name,
                        Units = Math.Round(position.CurrencyAmount, 4),
                        CurrentBuyRate = Math.Round(currentBuy, 4),
                        CurrentSellRate = Math.Round(currentSell, 4),
                        ValueInPln = Math.Round(valueInPln, 2),
                        DailyChangeValue = Math.Round(dailyChangeValue, 2),
                        DailyChangePercent = dailyChangePercent
                    };
                })
                .OrderByDescending(position => position.ValueInPln)
                .ToList();
        }

        private static IReadOnlyList<UserDashboardAlarmDto> BuildAlarmDtos(
            IEnumerable<UserAlarm> alarms,
            IReadOnlyDictionary<string, Currency> boardCurrencies)
        {
            return alarms
                .Select(alarm =>
                {
                    var hasCurrency = boardCurrencies.TryGetValue(alarm.ShortName, out var currency);
                    var currentBuy = hasCurrency ? currency!.BuyAt : 0m;
                    var currentSell = hasCurrency ? currency!.SellAt : 0m;
                    var buyDistance = alarm.AlarmBuyAt <= 0 || currentBuy <= 0
                        ? 0m
                        : Math.Round(alarm.AlarmBuyAt - currentBuy, 4);
                    var sellDistance = alarm.AlarmSellAt <= 0 || currentSell <= 0
                        ? 0m
                        : Math.Round(alarm.AlarmSellAt - currentSell, 4);

                    return new UserDashboardAlarmDto
                    {
                        CurrencyCode = alarm.ShortName,
                        AlarmBuyAt = alarm.AlarmBuyAt,
                        AlarmSellAt = alarm.AlarmSellAt,
                        CurrentBuyAt = currentBuy,
                        CurrentSellAt = currentSell,
                        DistanceToBuyAt = buyDistance,
                        DistanceToSellAt = sellDistance,
                        Status = ResolveAlarmStatus(alarm, currentBuy, currentSell)
                    };
                })
                .OrderBy(item => item.CurrencyCode)
                .ToList();
        }

        private static IReadOnlyList<UserDashboardInsightDto> BuildInsights(
            IReadOnlyList<UserDashboardPositionDto> positions,
            IReadOnlyList<UserDashboardAlarmDto> alarms,
            IReadOnlyList<TransactionHistory> transactions)
        {
            var insights = new List<UserDashboardInsightDto>();
            var strongestMover = positions
                .Where(position => !string.Equals(position.CurrencyCode, "PLN", StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(position => Math.Abs(position.DailyChangeValue))
                .FirstOrDefault();
            if (strongestMover is not null)
            {
                insights.Add(new UserDashboardInsightDto
                {
                    Title = $"{strongestMover.CurrencyCode} drives the wallet today",
                    Message = $"Your {strongestMover.CurrencyCode} position moved {strongestMover.DailyChangeValue:+0.00;-0.00;0.00} PLN today.",
                    Tone = strongestMover.DailyChangeValue >= 0 ? "positive" : "warning",
                    CurrencyCode = strongestMover.CurrencyCode,
                    Icon = strongestMover.DailyChangeValue >= 0 ? "ri-arrow-up-circle-line" : "ri-arrow-down-circle-line"
                });
            }

            var nearestAlarm = alarms
                .Where(alarm => alarm.AlarmBuyAt > 0 || alarm.AlarmSellAt > 0)
                .OrderBy(alarm =>
                {
                    var buyDistance = alarm.AlarmBuyAt > 0 ? Math.Abs(alarm.DistanceToBuyAt) : decimal.MaxValue;
                    var sellDistance = alarm.AlarmSellAt > 0 ? Math.Abs(alarm.DistanceToSellAt) : decimal.MaxValue;
                    return Math.Min(buyDistance, sellDistance);
                })
                .FirstOrDefault();
            if (nearestAlarm is not null)
            {
                insights.Add(new UserDashboardInsightDto
                {
                    Title = $"{nearestAlarm.CurrencyCode} is closest to an alert",
                    Message = nearestAlarm.Status,
                    Tone = "accent",
                    CurrencyCode = nearestAlarm.CurrencyCode,
                    Icon = "ri-notification-3-line"
                });
            }

            var latestTrade = transactions.OrderByDescending(item => item.DateOfTransaction).FirstOrDefault();
            if (latestTrade is not null)
            {
                insights.Add(new UserDashboardInsightDto
                {
                    Title = "Latest desk activity",
                    Message = $"{latestTrade.Description} {latestTrade.Amount:0.##} {latestTrade.CurrencyName} on {latestTrade.DateOfTransaction:dd MMM, HH:mm}.",
                    Tone = "neutral",
                    CurrencyCode = latestTrade.CurrencyName,
                    Icon = "ri-time-line"
                });
            }

            if (insights.Count == 0)
            {
                insights.Add(new UserDashboardInsightDto
                {
                    Title = "Workspace ready",
                    Message = "Top up the wallet or add an alert to start building your personal market cockpit.",
                    Tone = "neutral",
                    Icon = "ri-compass-3-line"
                });
            }

            return insights;
        }

        private static string ResolveAlarmStatus(UserAlarm alarm, decimal currentBuy, decimal currentSell)
        {
            if (alarm.AlarmBuyAt > 0 && currentBuy > 0)
            {
                if (currentBuy <= alarm.AlarmBuyAt)
                {
                    return $"Buy trigger reached at {currentBuy:0.0000}.";
                }

                return $"Buy target is {alarm.AlarmBuyAt:0.0000}; current buy sits {alarm.AlarmBuyAt - currentBuy:0.0000} PLN away.";
            }

            if (alarm.AlarmSellAt > 0 && currentSell > 0)
            {
                if (currentSell >= alarm.AlarmSellAt)
                {
                    return $"Sell trigger reached at {currentSell:0.0000}.";
                }

                return $"Sell target is {alarm.AlarmSellAt:0.0000}; current sell sits {alarm.AlarmSellAt - currentSell:0.0000} PLN away.";
            }

            return "No active threshold set yet.";
        }
    }
}
