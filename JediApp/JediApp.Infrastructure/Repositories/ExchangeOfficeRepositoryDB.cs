using JediApp.Database.Domain;
using JediApp.Database.Interface;
using JediApp.Web.Areas.Identity.Data;

namespace JediApp.Database.Repositories
{
    public class ExchangeOfficeRepositoryDB : IExchangeOfficeRepository
    {
        private readonly JediAppDbContext _jediAppDb;

        public ExchangeOfficeRepositoryDB(JediAppDbContext jediAppDb)
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