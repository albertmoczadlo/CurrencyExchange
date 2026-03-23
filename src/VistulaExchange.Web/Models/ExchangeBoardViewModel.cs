namespace VistulaExchange.Web.Models
{
    public sealed class ExchangeBoardViewModel
    {
        public string Title { get; init; } = string.Empty;
        public string Subtitle { get; init; } = string.Empty;
        public string SearchQuery { get; init; } = string.Empty;
        public bool IsAdminView { get; init; }
        public IReadOnlyList<CurrencyCardViewModel> Currencies { get; init; } = Array.Empty<CurrencyCardViewModel>();
    }
}
