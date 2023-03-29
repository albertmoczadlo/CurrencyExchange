using JediApp.Database.Domain;

namespace JediApp.Services.Services
{
    public interface IExchangeOfficeService
    {
        List<ExchangeOffice> GetAllExchangeOffices();
        ExchangeOffice GetExchangeOfficeById(Guid id);
        bool UpdateExchangeOffice(Guid id, ExchangeOffice exchangeOfficeToEdit);

    }
}