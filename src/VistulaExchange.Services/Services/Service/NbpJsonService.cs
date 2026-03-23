#nullable enable

using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;
using System.Threading.Tasks;

namespace VistulaExchange.Services.Services.Service
{
    public class NbpJsonService : INbpJsonService
    {
        private readonly INbpJsonRepository _nbpJsonRepository;

        public NbpJsonService(INbpJsonRepository nbpJsonRepository)
        {
            _nbpJsonRepository = nbpJsonRepository;
        }

        public Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return _nbpJsonRepository.GetAllCurrenciesAsync();
        }

        public Task<NBPJsonRoot?> GetNBPJsonTableInfoAsync()
        {
            return _nbpJsonRepository.GetNBPJsonTableInfoAsync();
        }

        public Task<List<Currency>> BrowseCurrencyAsync(string query)
        {
            return _nbpJsonRepository.BrowseCurrencyAsync(query);
        }

    }
}
