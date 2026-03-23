namespace VistulaExchange.Services.Models
{
    public sealed class UserDashboardPositionDto
    {
        public string CurrencyCode { get; init; } = string.Empty;
        public string CurrencyName { get; init; } = string.Empty;
        public decimal Units { get; init; }
        public decimal CurrentBuyRate { get; init; }
        public decimal CurrentSellRate { get; init; }
        public decimal ValueInPln { get; init; }
        public decimal DailyChangeValue { get; init; }
        public decimal DailyChangePercent { get; init; }
    }
}
