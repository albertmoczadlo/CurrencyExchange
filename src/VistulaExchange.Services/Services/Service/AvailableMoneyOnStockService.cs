using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Services.Services.Interfaces;

namespace VistulaExchange.Services.Services.Service
{
    public class AvailableMoneyOnStockService : IAvailableMoneyOnStockService
    {
        private readonly IAvailableMoneyOnStockRepository _availableMoneyOnStockRepository;

        public AvailableMoneyOnStockService(IAvailableMoneyOnStockRepository availableMoneyOnStockRepository)
        {
            _availableMoneyOnStockRepository = availableMoneyOnStockRepository;
        }

        public async Task<IReadOnlyList<MoneyOnStock>> GetAvailableMoneyOnStockAsync()
        {
            return await _availableMoneyOnStockRepository.GetAvailableMoneyOnStockAsync();
        }

        public Task AddMoneyToStockAsync(MoneyOnStock moneyOnStock)
        {
            return _availableMoneyOnStockRepository.AddMoneyToStockAsync(moneyOnStock);
        }

        public Task<string> SubtractMoneyFromStockAsync(MoneyOnStock moneyOnStock)
        {
            return _availableMoneyOnStockRepository.SubtractMoneyFromStockAsync(moneyOnStock);
        }
    }
}
