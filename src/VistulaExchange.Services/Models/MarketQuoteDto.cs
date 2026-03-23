namespace VistulaExchange.Services.Models
{
    public sealed class MarketQuoteDto
    {
        public Guid Id { get; init; }
        public string CurrencyCode { get; init; } = string.Empty;
        public string CurrencyName { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public decimal BuyAt { get; init; }
        public decimal SellAt { get; init; }
        public decimal Spread => Math.Round(SellAt - BuyAt, 4);
    }
}
