namespace JediApp.Database.Domain
{
    public class ExchangeOfficeBoard
    {
        public ExchangeOfficeBoard()
        {
            Id= Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ExchangeOfficeId { get; set; }

        public ExchangeOffice ExchangeOffice { get; set; }
        public ICollection<Currency> Currencies { get; set; }
    }
}
