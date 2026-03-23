namespace VistulaExchange.Web.Models
{
    public sealed class QuickExchangeViewModel
    {
        public Guid CurrencyId { get; set; }
        public string CurrencyName { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        public string SourceCurrencyCode { get; set; } = string.Empty;
        public string TargetCurrencyCode { get; set; } = string.Empty;
        public decimal SourceBalance { get; set; }
        public decimal TargetBalance { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal Rate { get; set; }
        public decimal FeeRate { get; set; }
        public decimal GrossValue { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal NetValue { get; set; }
        public decimal TargetLiquidityAvailable { get; set; }
        public DateTime QuoteLockedUntilUtc { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public bool IsBuy => string.Equals(Mode, "Buy", StringComparison.OrdinalIgnoreCase);
        public bool QuoteExpired => DateTime.UtcNow >= QuoteLockedUntilUtc;
    }
}
