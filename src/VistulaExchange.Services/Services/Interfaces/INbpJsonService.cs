#nullable enable

using System.Threading.Tasks;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Models;
using VistulaExchange.Services.Models;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface INbpJsonService
    {
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task<NBPJsonRoot?> GetNBPJsonTableInfoAsync();
        Task<List<Currency>> BrowseCurrencyAsync(string query);
        Task<IReadOnlyList<CurrencyHistoryPoint>> GetCurrencyHistoryAsync(string currencyCode, int days);
    }
}
