using JediApp.Database.Domain;

namespace JediApp.Web.Models
{
    public class AdminReport
    {
        //public decimal AnnualTurnover { get; set; }
        //public decimal MonthlyTurnover { get; set; }
        //public decimal DailyTurnover { get; set; }
        public decimal TurnoverPLN { get; set; }
        public decimal TurnoverEUR { get; set; }
        public decimal TurnoverUSD { get; set; }
        public decimal TurnoverGBP { get; set; }
        public decimal TurnoverCHF { get; set; }
        public decimal TurnoverCZK { get; set; }
        public int AnnualTransactions { get; set; }
        public int MonthlyTransactions { get; set; }
        public int DailyTTransactions { get; set; }        
        public int UserAccounts { get; set; }
        public int UserEmailConfirmed { get; set; }


    }
}
