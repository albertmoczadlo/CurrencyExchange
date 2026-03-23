using VistulaExchange.Database.Domain;

namespace VistulaExchange.Web.Models
{
    public sealed class CurrencyCardViewModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string ShortName { get; init; } = string.Empty;
        public string Country { get; init; } = string.Empty;
        public decimal BuyAt { get; init; }
        public decimal SellAt { get; init; }
        public decimal Spread => Math.Round(SellAt - BuyAt, 4);

        public string SpreadProfile =>
            Spread switch
            {
                <= 0.15m => "Tight spread",
                <= 0.35m => "Balanced spread",
                _ => "Premium spread"
            };

        public static CurrencyCardViewModel FromCurrency(Currency currency)
        {
            return new CurrencyCardViewModel
            {
                Id = currency.Id,
                Name = currency.Name,
                ShortName = currency.ShortName,
                Country = currency.Country,
                BuyAt = currency.BuyAt,
                SellAt = currency.SellAt
            };
        }
    }
}
