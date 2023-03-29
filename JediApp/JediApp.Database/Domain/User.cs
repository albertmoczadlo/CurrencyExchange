using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace JediApp.Database.Domain
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public Guid? ExchangeOfficeId { get; set; }

        //public ExchangeOffice ExchangeOffice { get; set; }
        public virtual Wallet Wallet { get; set; }
        public ICollection<TransactionHistory> TransactionHistory { get; set; }
        public ICollection<UserAlarm> UserAlarms { get; set; }
    }


}
