namespace VistulaExchange.Database.Models
{
    public sealed class CurrencyHistoryPoint
    {
        public DateTime Date { get; init; }
        public decimal BuyAt { get; init; }
        public decimal SellAt { get; init; }
    }
}
