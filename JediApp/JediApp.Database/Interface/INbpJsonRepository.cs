using JediApp.Database.Domain;

namespace JediApp.Database.Interface
{
    public interface INbpJsonRepository
    {
        List<Currency> GetAllCurrencies();
        NBPJsonRoot GetNBPJsonTableInfo();
        List<Currency> BrowseCurrency(string query);
    }
}