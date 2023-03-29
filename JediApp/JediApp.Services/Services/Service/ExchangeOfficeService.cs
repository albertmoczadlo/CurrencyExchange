using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Services.Services.Interfaces;

namespace JediApp.Services.Services.Service
{
    public class ExchangeOfficeService : IExchangeOfficeService
    {
        private readonly IExchangeOfficeRepository _exchangeOfficeRepository;

        public ExchangeOfficeService(IExchangeOfficeRepository exchangeOfficeRepository)
        {
            _exchangeOfficeRepository = exchangeOfficeRepository;
        }

        public List<ExchangeOffice> GetAllExchangeOffices()
        {
            return _exchangeOfficeRepository.GetAllExchangeOffices();
        }

        public ExchangeOffice GetExchangeOfficeById(Guid id)
        {
            return _exchangeOfficeRepository.GetExchangeOfficeById(id);
        }

        public bool UpdateExchangeOffice(Guid id, ExchangeOffice exchangeOfficeToEdit)
        {
            var exchangeOffice = GetAllExchangeOffices().FirstOrDefault(c => c.Id == id);

            if (exchangeOffice == null)
            {
                return false;
            }
            return _exchangeOfficeRepository.UpdateExchangeOffice(id, exchangeOfficeToEdit);
        }

    }
}
