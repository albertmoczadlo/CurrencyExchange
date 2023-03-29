using System.ComponentModel.DataAnnotations.Schema;

namespace JediApp.Database.Domain
{
    public class UserAlarm
    {
        public UserAlarm()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public string CurrencyName { get; set; }
        public string ShortName { get; set; }
        public decimal AlarmBuyAt { get; set; }
        public decimal AlarmSellAt { get; set; }

    }
}
