using JediApp.Database.Domain;
using JediApp.Database.Interface;

namespace JediApp.Services.Services
{
    public class NbpJsonService : INbpJsonService
    {
        private readonly INbpJsonRepository _nbpJsonRepository;

        public NbpJsonService(INbpJsonRepository nbpJsonRepository)
        {
            _nbpJsonRepository = nbpJsonRepository;
        }

        public List<Currency> GetAllCurrencies()
        {
            return _nbpJsonRepository.GetAllCurrencies();
        }       
        
        public NBPJsonRoot GetNBPJsonTableInfo()
        {
            return _nbpJsonRepository.GetNBPJsonTableInfo();
        }

        public List<Currency> BrowseCurrency(string query)
        {
            return _nbpJsonRepository.BrowseCurrency(query);
        }

    }
}
