namespace VistulaExchange.Services.Models
{
    public sealed class UserDashboardInsightDto
    {
        public string Title { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string Tone { get; init; } = "neutral";
        public string CurrencyCode { get; init; } = string.Empty;
        public string Icon { get; init; } = "ri-line-chart-line";
    }
}
