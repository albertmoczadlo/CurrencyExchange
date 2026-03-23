namespace VistulaExchange.Services.Models
{
    public sealed class AdminLiquidityDto
    {
        public string CurrencyCode { get; init; } = string.Empty;
        public decimal Value { get; init; }
    }
}
