#nullable enable

using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Database.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VistulaExchange.Infrastructure.Repositories
{
    public class NbpJsonRepository : INbpJsonRepository
    {
        private const string ApiPath = "https://api.nbp.pl/api/exchangerates/tables/C/";
        private const string HistoryApiTemplate = "https://api.nbp.pl/api/exchangerates/rates/c/{0}/last/{1}/?format=json";
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

        public async Task<IReadOnlyList<CurrencyHistoryPoint>> GetCurrencyHistoryAsync(string currencyCode, int days)
        {
            var normalizedCurrencyCode = (currencyCode ?? string.Empty).Trim().ToUpperInvariant();
            if (string.Equals(normalizedCurrencyCode, "PLN", StringComparison.OrdinalIgnoreCase))
            {
                var safeDays = Math.Clamp(days, 1, 90);
                return Enumerable.Range(0, safeDays)
                    .Select(offset => new CurrencyHistoryPoint
                    {
                        Date = DateTime.Today.AddDays(offset - safeDays + 1),
                        BuyAt = 1m,
                        SellAt = 1m
                    })
                    .ToList();
            }

            var safeCurrencyCode = Uri.EscapeDataString(normalizedCurrencyCode);
            var safeDaysCount = Math.Clamp(days, 1, 90);
            var requestUri = string.Format(HistoryApiTemplate, safeCurrencyCode, safeDaysCount);
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var history = JsonConvert.DeserializeObject<NbpCurrencySeriesResponse>(result);
            if (history?.Rates is null)
            {
                return Array.Empty<CurrencyHistoryPoint>();
            }

            return history.Rates
                .Select(rate => new CurrencyHistoryPoint
                {
                    Date = rate.EffectiveDate,
                    BuyAt = rate.Ask,
                    SellAt = rate.Bid
                })
                .OrderBy(point => point.Date)
                .ToList();
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

        private sealed class NbpCurrencySeriesResponse
        {
            [JsonProperty("rates")]
            public List<NbpCurrencySeriesRate> Rates { get; set; } = new();
        }

        private sealed class NbpCurrencySeriesRate
        {
            [JsonProperty("effectiveDate")]
            public DateTime EffectiveDate { get; set; }

            [JsonProperty("bid")]
            public decimal Bid { get; set; }

            [JsonProperty("ask")]
            public decimal Ask { get; set; }
        }
    }
}
