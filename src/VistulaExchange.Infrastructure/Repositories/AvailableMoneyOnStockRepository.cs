using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class AvailableMoneyOnStockRepository : IAvailableMoneyOnStockRepository
    {
        private readonly VistulaExchangeDbContext _dbContext;

        public AvailableMoneyOnStockRepository(VistulaExchangeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddMoneyToStockAsync(MoneyOnStock moneyOnStock)
        {
            var office = await _dbContext.ExchangeOffices.FirstOrDefaultAsync();
            moneyOnStock.ExchangeOffice = office;

            var currentMoneyOnStockForCurrency = await _dbContext.MoneyOnStocks
                .SingleOrDefaultAsync(a => a.CurrencyName == moneyOnStock.CurrencyName);
            if (currentMoneyOnStockForCurrency != null)
            {
                currentMoneyOnStockForCurrency.Value += moneyOnStock.Value;
            }
            else
            {
                await _dbContext.MoneyOnStocks.AddAsync(moneyOnStock);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> SubtractMoneyFromStockAsync(MoneyOnStock moneyOnStock)
        {
            var office = await _dbContext.ExchangeOffices.FirstOrDefaultAsync();
            moneyOnStock.ExchangeOffice = office;

            var currentMoneyOnStockForCurrency = await _dbContext.MoneyOnStocks
                .SingleOrDefaultAsync(a => a.CurrencyName == moneyOnStock.CurrencyName);

            if (currentMoneyOnStockForCurrency != null)
            {
                currentMoneyOnStockForCurrency.Value -= moneyOnStock.Value;
                if (currentMoneyOnStockForCurrency.Value < 0)
                {
                    return "Not enough money in the account.";
                }
            }
            else
            {
                return "No money found for this currency.";
            }

            await _dbContext.SaveChangesAsync();

            return " ";
        }


        public Task<List<MoneyOnStock>> GetAvailableMoneyOnStockAsync()
        {
            return _dbContext.MoneyOnStocks.ToListAsync();
        }

    }
}
