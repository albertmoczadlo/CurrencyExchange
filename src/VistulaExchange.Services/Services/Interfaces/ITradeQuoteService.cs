#nullable enable
using VistulaExchange.Services.Models;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface ITradeQuoteService
    {
        Task<TradeQuoteDto?> BuildQuoteAsync(string userId, Guid currencyId, string mode, decimal? requestedAmount = null);
    }
}
