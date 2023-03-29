using System.ComponentModel.DataAnnotations.Schema;

namespace JediApp.Database.Domain
{
    public class TransactionHistory
    {
        public TransactionHistory()
        {
            Id= Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public string CurrencyName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public string Description { get; set; }

        public User User { get; set; }

    }
}
