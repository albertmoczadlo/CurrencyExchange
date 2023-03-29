namespace JediApp.Database.Domain
{
    public class MoneyOnStock
    {
        public Guid Id { get; set; }
        public string CurrencyName { get; set; }
        public decimal Value { get; set; }
        public Guid ExchangeOfficeId { get; set; }

        public ExchangeOffice ExchangeOffice { get; set; }
    }
}
