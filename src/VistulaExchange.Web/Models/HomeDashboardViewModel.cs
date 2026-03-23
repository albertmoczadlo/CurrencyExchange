namespace VistulaExchange.Web.Models
{
    public sealed class HomeDashboardViewModel
    {
        public IReadOnlyList<CurrencyCardViewModel> Currencies { get; init; } = Array.Empty<CurrencyCardViewModel>();
        public IReadOnlyList<CurrencyCardViewModel> SpotlightCurrencies { get; init; } = Array.Empty<CurrencyCardViewModel>();
        public CurrencyCardViewModel? BestBuyCurrency { get; init; }
        public CurrencyCardViewModel? BestSellCurrency { get; init; }
        public CurrencyCardViewModel? TightestSpreadCurrency { get; init; }
        public decimal AverageSpread { get; init; }
        public bool IsAuthenticated { get; init; }
        public bool IsAdmin { get; init; }
        public string UserDisplayName { get; init; } = string.Empty;
    }
}
