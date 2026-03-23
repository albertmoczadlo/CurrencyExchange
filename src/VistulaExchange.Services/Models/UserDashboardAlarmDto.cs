namespace VistulaExchange.Services.Models
{
    public sealed class UserDashboardAlarmDto
    {
        public string CurrencyCode { get; init; } = string.Empty;
        public decimal AlarmBuyAt { get; init; }
        public decimal AlarmSellAt { get; init; }
        public decimal CurrentBuyAt { get; init; }
        public decimal CurrentSellAt { get; init; }
        public decimal DistanceToBuyAt { get; init; }
        public decimal DistanceToSellAt { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
