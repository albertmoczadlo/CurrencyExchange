namespace VistulaExchange.Web.Models
{
    public sealed class CurrencyDetailsViewModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public decimal BuyAt { get; init; }
        public decimal SellAt { get; init; }
        public decimal Spread => Math.Round(SellAt - BuyAt, 4);
    }
}
