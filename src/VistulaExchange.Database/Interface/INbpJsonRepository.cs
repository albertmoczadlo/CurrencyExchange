#nullable enable

using System.Threading.Tasks;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Models;

namespace VistulaExchange.Database.Interface
{
    public interface INbpJsonRepository
    {
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task<NBPJsonRoot?> GetNBPJsonTableInfoAsync();
        Task<List<Currency>> BrowseCurrencyAsync(string query);
        Task<IReadOnlyList<CurrencyHistoryPoint>> GetCurrencyHistoryAsync(string currencyCode, int days);
    }
}
