using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace JediApp.Database.Domain
{
    public class Wallet 
    {
        public Wallet()
        {
            Id= Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }
        public ICollection<WalletPosition> WalletPositions { get; set; } 
        
    }
   
}
