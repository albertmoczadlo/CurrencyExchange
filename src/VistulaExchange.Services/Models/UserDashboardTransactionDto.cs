namespace VistulaExchange.Services.Models
{
    public sealed class UserDashboardTransactionDto
    {
        public string CurrencyCode { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime DateOfTransaction { get; init; }
        public string Description { get; init; } = string.Empty;
        public string UserDisplayName { get; init; } = string.Empty;
        public string UserEmail { get; init; } = string.Empty;
    }
}
