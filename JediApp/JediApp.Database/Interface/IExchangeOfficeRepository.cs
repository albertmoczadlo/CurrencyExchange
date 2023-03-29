using JediApp.Database.Domain;

namespace JediApp.Database.Interface
{
    public interface IExchangeOfficeRepository
    {
        List<ExchangeOffice> GetAllExchangeOffices();
        ExchangeOffice GetExchangeOfficeById(Guid id);
        bool UpdateExchangeOffice(Guid id, ExchangeOffice exchangeOfficeToEdit);
    }
}