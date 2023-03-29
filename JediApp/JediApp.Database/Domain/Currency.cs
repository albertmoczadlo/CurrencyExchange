using System.Collections.Generic;

namespace JediApp.Database.Domain
{
    public class Currency 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Country { get; set; }
        public decimal BuyAt { get; set; }
        public decimal SellAt { get; set; }

        //public Guid WalletPositionId { get; set; }
        public Guid ExchangeOfficeBoardId { get; set; }

        public ExchangeOfficeBoard ExchangeOfficeBoard { get; set; }
        //public WalletPosition WalletPosition { get; set; }


    }
}
