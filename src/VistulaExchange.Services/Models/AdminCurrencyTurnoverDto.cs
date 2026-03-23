namespace VistulaExchange.Services.Models
{
    public sealed class AdminCurrencyTurnoverDto
    {
        public string CurrencyCode { get; init; } = string.Empty;
        public decimal Amount { get; init; }
    }
}
