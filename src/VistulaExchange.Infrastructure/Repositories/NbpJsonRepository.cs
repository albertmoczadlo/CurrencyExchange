#nullable enable

using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VistulaExchange.Database.Repositories
{
    public class NbpJsonRepository : INbpJsonRepository
    {
        private const string ApiPath = "http://api.nbp.pl/api/exchangerates/tables/C/";
        private static readonly HashSet<string> BaseCurrenciesInExchangeOffice = new(StringComparer.OrdinalIgnoreCase)
        {
            "AUD", "CAD", "CHF", "CZK", "DKK", "EUR", "GBP", "HUF", "JPY", "NOK", "PLN", "SEK", "USD"
        };
        private readonly HttpClient _httpClient;

        public NbpJsonRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            var currencies = new List<Currency>();
            var nbpJson = await GetTableAsync();

            foreach (var item in nbpJson)
            {
                foreach (var currency in item.rates)
                {
                    if (BaseCurrenciesInExchangeOffice.Contains(currency.code))
                    {
                        currencies.Add(new Currency
                        {
                            Id = Guid.NewGuid(),
                            Name = currency.currency,
                            ShortName = currency.code,
                            Country = "bd",
                            BuyAt = currency.ask,
                            SellAt = currency.bid
                        });
                    }
                }
            }

            return currencies.OrderBy(c => c.ShortName).ToList();
        }

        public async Task<NBPJsonRoot?> GetNBPJsonTableInfoAsync()
        {
            var nbpJson = await GetTableAsync();
            return nbpJson.FirstOrDefault();
        }

        public async Task<List<Currency>> BrowseCurrencyAsync(string query)
        {
            var normalizedQuery = query?.Trim() ?? string.Empty;
            var currencies = await GetAllCurrenciesAsync();

            if (string.IsNullOrWhiteSpace(normalizedQuery))
            {
                return currencies;
            }

            return currencies.Where(x => x.Name.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                                      || x.ShortName.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)
                                      || x.Country.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        private async Task<List<NBPJsonRoot>> GetTableAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ApiPath);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NBPJsonRoot>>(result) ?? new List<NBPJsonRoot>();
        }
    }
}
