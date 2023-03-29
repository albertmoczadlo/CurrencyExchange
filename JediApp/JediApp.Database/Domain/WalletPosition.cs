using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediApp.Database.Domain
{
    public class WalletPosition
    {
        public Guid Id { get; set; }
        public decimal CurrencyAmount { get; set; }

        public Guid WalletId { get; set; }
        public Guid CurrencyId { get; set; }

        public Wallet Wallet { get; set; }
        public Currency Currency { get; set; }

    }
}
