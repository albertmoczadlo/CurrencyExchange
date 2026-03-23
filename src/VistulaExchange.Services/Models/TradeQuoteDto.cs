namespace VistulaExchange.Services.Models
{
    public sealed class TradeQuoteDto
    {
        public Guid CurrencyId { get; init; }
        public string CurrencyName { get; init; } = string.Empty;
        public string Mode { get; init; } = string.Empty;
        public string SourceCurrencyCode { get; init; } = string.Empty;
        public string TargetCurrencyCode { get; init; } = string.Empty;
        public decimal SourceBalance { get; init; }
        public decimal TargetBalance { get; init; }
        public decimal RequestedAmount { get; init; }
        public decimal MaxAmount { get; init; }
        public decimal Rate { get; init; }
        public decimal FeeRate { get; init; }
        public decimal GrossValue { get; init; }
        public decimal FeeAmount { get; init; }
        public decimal NetValue { get; init; }
        public decimal TargetLiquidityAvailable { get; init; }
        public DateTime QuoteLockedUntilUtc { get; init; }
    }
}
