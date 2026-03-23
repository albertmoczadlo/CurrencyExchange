namespace VistulaExchange.Services.Models
{
    public sealed class UserDashboardDto
    {
        public string UserDisplayName { get; init; } = string.Empty;
        public decimal TotalValueInPln { get; init; }
        public decimal DailyChangeValue { get; init; }
        public decimal DailyChangePercent { get; init; }
        public string DefaultChartCurrencyCode { get; init; } = string.Empty;
        public IReadOnlyList<UserDashboardPositionDto> Positions { get; init; } = Array.Empty<UserDashboardPositionDto>();
        public IReadOnlyList<UserDashboardTransactionDto> RecentTransactions { get; init; } = Array.Empty<UserDashboardTransactionDto>();
        public IReadOnlyList<UserDashboardAlarmDto> ActiveAlarms { get; init; } = Array.Empty<UserDashboardAlarmDto>();
        public IReadOnlyList<UserDashboardInsightDto> Insights { get; init; } = Array.Empty<UserDashboardInsightDto>();
        public IReadOnlyList<MarketQuoteDto> MarketQuotes { get; init; } = Array.Empty<MarketQuoteDto>();
    }
}
