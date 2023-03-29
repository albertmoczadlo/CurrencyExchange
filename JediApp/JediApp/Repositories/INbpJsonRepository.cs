using JediApp.Database.Domain;

namespace JediApp.Database.Repositories
{
    public interface INbpJsonRepository
    {
        List<Currency> GetAllCurrencies();
        NBPJsonRoot GetNBPJsonTableInfo();
        List<Currency> BrowseCurrency(string query);
    }
}