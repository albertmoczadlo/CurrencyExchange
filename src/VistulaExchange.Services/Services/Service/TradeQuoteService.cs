#nullable enable
using VistulaExchange.Database.Domain;
using VistulaExchange.Services.Models;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class TradeQuoteService : ITradeQuoteService
    {
        private const decimal DefaultFeeRate = 0.005m;
        private const decimal MaxFeeRate = 0.05m;

        private readonly IExchangeOfficeBoardService _exchangeOfficeBoardService;
        private readonly IUserWalletService _userWalletService;
        private readonly IAvailableMoneyOnStockService _availableMoneyOnStockService;
        private readonly IExchangeOfficeService _exchangeOfficeService;

        public TradeQuoteService(
            IExchangeOfficeBoardService exchangeOfficeBoardService,
            IUserWalletService userWalletService,
            IAvailableMoneyOnStockService availableMoneyOnStockService,
            IExchangeOfficeService exchangeOfficeService)
        {
            _exchangeOfficeBoardService = exchangeOfficeBoardService;
            _userWalletService = userWalletService;
            _availableMoneyOnStockService = availableMoneyOnStockService;
            _exchangeOfficeService = exchangeOfficeService;
        }

        public async Task<TradeQuoteDto?> BuildQuoteAsync(string userId, Guid currencyId, string mode, decimal? requestedAmount = null)
        {
            var tradeMode = string.Equals(mode, "sell", StringComparison.OrdinalIgnoreCase) ? "Sell" : "Buy";
            var currencies = _exchangeOfficeBoardService.GetAllCurrencies();
            var mainCurrency = currencies.FirstOrDefault(currency => currency.Id == currencyId);

            if (mainCurrency is null)
            {
                return null;
            }

            var feeRate = ResolveFeeRate();
            var stock = await _availableMoneyOnStockService.GetAvailableMoneyOnStockAsync();

            return tradeMode == "Buy"
                ? await BuildBuyQuoteAsync(userId, mainCurrency, feeRate, stock, requestedAmount)
                : await BuildSellQuoteAsync(userId, mainCurrency, feeRate, stock, requestedAmount);
        }

        private async Task<TradeQuoteDto> BuildBuyQuoteAsync(
            string userId,
            Currency targetCurrency,
            decimal feeRate,
            IReadOnlyList<MoneyOnStock> stock,
            decimal? requestedAmount)
        {
            var plnBalance = (await _userWalletService.GetCurrencyBalanceByCodeAsync(userId, "PLN")).CurrencyAmount;
            var targetBalance = (await _userWalletService.GetCurrencyBalanceByCodeAsync(userId, targetCurrency.ShortName)).CurrencyAmount;
            var targetStock = stock.FirstOrDefault(item => string.Equals(item.CurrencyName, targetCurrency.ShortName, StringComparison.OrdinalIgnoreCase))?.Value ?? 0m;
            var affordableAmount = targetCurrency.BuyAt <= 0
                ? 0m
                : plnBalance / (targetCurrency.BuyAt * (1m + feeRate));
            var maxAmount = Math.Max(0m, Math.Min(targetStock, affordableAmount));
            var defaultAmount = maxAmount <= 0 ? 0m : Math.Min(maxAmount, 100m);
            var amount = ClampAmount(requestedAmount ?? defaultAmount, maxAmount);
            var grossValue = Math.Round(amount * targetCurrency.BuyAt, 2);
            var feeAmount = Math.Round(grossValue * feeRate, 2);

            return new TradeQuoteDto
            {
                CurrencyId = targetCurrency.Id,
                CurrencyName = targetCurrency.Name,
                Mode = "Buy",
                SourceCurrencyCode = "PLN",
                TargetCurrencyCode = targetCurrency.ShortName,
                SourceBalance = Math.Round(plnBalance, 2),
                TargetBalance = Math.Round(targetBalance, 4),
                RequestedAmount = amount,
                MaxAmount = Math.Round(maxAmount, 4),
                Rate = targetCurrency.BuyAt,
                FeeRate = feeRate,
                GrossValue = grossValue,
                FeeAmount = feeAmount,
                NetValue = grossValue + feeAmount,
                TargetLiquidityAvailable = Math.Round(targetStock, 4),
                QuoteLockedUntilUtc = DateTime.UtcNow.AddSeconds(30)
            };
        }

        private async Task<TradeQuoteDto> BuildSellQuoteAsync(
            string userId,
            Currency sourceCurrency,
            decimal feeRate,
            IReadOnlyList<MoneyOnStock> stock,
            decimal? requestedAmount)
        {
            var sourceBalance = (await _userWalletService.GetCurrencyBalanceByCodeAsync(userId, sourceCurrency.ShortName)).CurrencyAmount;
            var plnBalance = (await _userWalletService.GetCurrencyBalanceByCodeAsync(userId, "PLN")).CurrencyAmount;
            var plnStock = stock.FirstOrDefault(item => string.Equals(item.CurrencyName, "PLN", StringComparison.OrdinalIgnoreCase))?.Value ?? 0m;
            var payoutFactor = sourceCurrency.SellAt * (1m - feeRate);
            var liquidityLimitedAmount = payoutFactor <= 0 ? 0m : plnStock / payoutFactor;
            var maxAmount = Math.Max(0m, Math.Min(sourceBalance, liquidityLimitedAmount));
            var defaultAmount = maxAmount <= 0 ? 0m : Math.Min(maxAmount, 100m);
            var amount = ClampAmount(requestedAmount ?? defaultAmount, maxAmount);
            var grossValue = Math.Round(amount * sourceCurrency.SellAt, 2);
            var feeAmount = Math.Round(grossValue * feeRate, 2);

            return new TradeQuoteDto
            {
                CurrencyId = sourceCurrency.Id,
                CurrencyName = sourceCurrency.Name,
                Mode = "Sell",
                SourceCurrencyCode = sourceCurrency.ShortName,
                TargetCurrencyCode = "PLN",
                SourceBalance = Math.Round(sourceBalance, 4),
                TargetBalance = Math.Round(plnBalance, 2),
                RequestedAmount = amount,
                MaxAmount = Math.Round(maxAmount, 4),
                Rate = sourceCurrency.SellAt,
                FeeRate = feeRate,
                GrossValue = grossValue,
                FeeAmount = feeAmount,
                NetValue = grossValue - feeAmount,
                TargetLiquidityAvailable = Math.Round(plnStock, 2),
                QuoteLockedUntilUtc = DateTime.UtcNow.AddSeconds(30)
            };
        }

        private decimal ResolveFeeRate()
        {
            var office = _exchangeOfficeService.GetAllExchangeOffices().FirstOrDefault();
            if (office is null)
            {
                return DefaultFeeRate;
            }

            var derivedRate = office.Markup / 1000m;
            return Math.Round(Math.Clamp(derivedRate, 0m, MaxFeeRate), 4);
        }

        private static decimal ClampAmount(decimal requestedAmount, decimal maxAmount)
        {
            if (maxAmount <= 0)
            {
                return 0;
            }

            return Math.Round(Math.Clamp(requestedAmount, 0m, maxAmount), 4);
        }
    }
}
