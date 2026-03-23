using System.ComponentModel.DataAnnotations;

namespace VistulaExchange.Web.Models
{
    public class UserWithdrawal
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
    }
}
