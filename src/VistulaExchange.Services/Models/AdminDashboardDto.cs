namespace VistulaExchange.Services.Models
{
    public sealed class AdminDashboardDto
    {
        public int UserAccounts { get; init; }
        public int UserEmailConfirmed { get; init; }
        public int DailyTransactions { get; init; }
        public int MonthlyTransactions { get; init; }
        public int AnnualTransactions { get; init; }
        public decimal AverageSpread { get; init; }
        public string HottestCurrencyCode { get; init; } = string.Empty;
        public int HottestCurrencyTransactions { get; init; }
        public IReadOnlyList<AdminCurrencyTurnoverDto> Turnovers { get; init; } = Array.Empty<AdminCurrencyTurnoverDto>();
        public IReadOnlyList<AdminLiquidityDto> Liquidity { get; init; } = Array.Empty<AdminLiquidityDto>();
        public IReadOnlyList<AdminTimelinePointDto> MonthlyActivity { get; init; } = Array.Empty<AdminTimelinePointDto>();
        public IReadOnlyList<UserDashboardTransactionDto> LatestTransactions { get; init; } = Array.Empty<UserDashboardTransactionDto>();
    }
}
