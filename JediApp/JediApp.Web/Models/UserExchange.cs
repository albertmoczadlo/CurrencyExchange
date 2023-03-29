namespace JediApp.Web.Models
{
    public class UserExchange
    {
        public string ExchangeFromCurrency { get; set; }
        public decimal ExchangeFromCurrencyAmount { get; set; }
        public string ExchangeToCurrency { get; set; }
        public decimal ExchangeToCurrencyAmount { get; set; }
        public decimal BuyAt { get; set; }
        public decimal SellAt { get; set; }
        public decimal ExchangeMaxAmount { get; set; }
    }
}
