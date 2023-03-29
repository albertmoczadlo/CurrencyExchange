using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JediApp.Database.Domain
{
    public class ExchangeOffice
    {
        public ExchangeOffice()
        {
            Id= Guid.NewGuid();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public int Markup { get; set; } //marża, narzut

        public Guid Id { get; set; }
        //public string? UserId { get; set; }
        

        public ExchangeOfficeBoard ExchangeOfficeBoard { get; set; }
        //public User User { get; set; }
        public ICollection<MoneyOnStock> MoneyOnStocks { get; set; }
    }
}
