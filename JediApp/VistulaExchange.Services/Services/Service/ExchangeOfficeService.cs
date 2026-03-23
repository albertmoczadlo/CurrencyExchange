using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
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
