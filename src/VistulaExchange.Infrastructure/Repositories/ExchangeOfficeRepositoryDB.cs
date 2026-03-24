using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class ExchangeOfficeRepositoryDB : IExchangeOfficeRepository
    {
        private readonly VistulaExchangeDbContext _dbContext;

        public ExchangeOfficeRepositoryDB(VistulaExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ExchangeOffice> GetAllExchangeOffices()
        {

            return _dbContext.ExchangeOffices.OrderBy(c => c.Name).ToList();

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

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ExchangeOfficeRepository] UpdateExchangeOffice failed for id={id}: {ex}");
                return false;
            }
            return true;
        }

    }
}