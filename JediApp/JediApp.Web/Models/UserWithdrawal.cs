using System.ComponentModel.DataAnnotations;

namespace JediApp.Web.Models
{
    public class UserWithdrawal
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
