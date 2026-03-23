#nullable enable

using System.Threading.Tasks;
using VistulaExchange.Database.Domain;

namespace VistulaExchange.Services.Services.Interfaces
{
    public interface INbpJsonService
    {
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task<NBPJsonRoot?> GetNBPJsonTableInfoAsync();
        Task<List<Currency>> BrowseCurrencyAsync(string query);
    }
}
