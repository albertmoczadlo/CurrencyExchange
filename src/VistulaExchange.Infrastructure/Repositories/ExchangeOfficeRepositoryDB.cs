using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class ExchangeOfficeRepositoryDB : IExchangeOfficeRepository
    {
        private readonly VistulaExchangeDbContext _jediAppDb;

        public ExchangeOfficeRepositoryDB(VistulaExchangeDbContext jediAppDb)
        {
            _jediAppDb = jediAppDb;
        }

        public List<ExchangeOffice> GetAllExchangeOffices()
        {

            return _jediAppDb.ExchangeOffices.OrderBy(c => c.Name).ToList();

        }


        public ExchangeOffice GetExchangeOfficeById(Guid id)
        {
            List<ExchangeOffice> exchangeOffice = GetAllExchangeOffices();

            return exchangeOffice.SingleOrDefault(x => x.Id == id);
        }

        public bool UpdateExchangeOffice(Guid id, ExchangeOffice exchangeOfficeToEdit)
        {
            try
            {
                var exchangeOffice = GetExchangeOfficeById(id);

                exchangeOffice.Name = exchangeOfficeToEdit.Name;
                exchangeOffice.Address = exchangeOfficeToEdit.Address;
                exchangeOffice.Markup = exchangeOfficeToEdit.Markup;

                //_jediAppDb.SaveChangesAsync();
                _jediAppDb.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}