using JediApp.Database.Domain;

namespace JediApp.Services.Services
{
    public interface INbpJsonService
    {
        List<Currency> GetAllCurrencies();
        NBPJsonRoot GetNBPJsonTableInfo();
        List<Currency> BrowseCurrency(string query);
    }
}