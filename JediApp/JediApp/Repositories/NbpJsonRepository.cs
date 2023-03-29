using JediApp.Database.Domain;
using Newtonsoft.Json;
using System.Net;

namespace JediApp.Database.Repositories
{
    public class NbpJsonRepository : INbpJsonRepository
    {
        private readonly string apiPath = "http://api.nbp.pl/api/exchangerates/tables/C/"; //może przeniść do klasy statycznej ???

        public List<Currency> GetAllCurrencies()
        {
            List<Currency> currencies = new();

            using (var client = new WebClient()) //WebClient  
            {
                client.Headers.Add("Content-Type:application/json"); //Content-Type  
                client.Headers.Add("Accept:application/json");
                var result = client.DownloadString(apiPath);                                                                                                    

                List<NBPJsonRoot> NBPJson = JsonConvert.DeserializeObject<List<NBPJsonRoot>>(result);

                foreach (var item in NBPJson)
                {
                    var NBPJsonRate = item.rates;
                    foreach (var currency in NBPJsonRate)
                    {
                        //decimal.TryParse(currency.ask, out var ask);
                        //decimal.TryParse(currency.bid, out var bid);
                        currencies.Add(new Currency { Id = Guid.NewGuid(), Name = currency.currency, ShortName = currency.code, Country = " ", BuyAt = currency.ask, SellAt = currency.bid });

                    }
                }

            }

            return currencies.OrderBy(c => c.ShortName).ToList();
        }

        public NBPJsonRoot GetNBPJsonTableInfo()
        {
            List<NBPJsonRoot> NBPJson = default;

            using (var client = new WebClient()) //WebClient  
            {
                client.Headers.Add("Content-Type:application/json"); //Content-Type  
                client.Headers.Add("Accept:application/json");
                var result = client.DownloadString(apiPath);

                NBPJson = JsonConvert.DeserializeObject<List<NBPJsonRoot>>(result);

            }

            return NBPJson.FirstOrDefault();
        }

        public List<Currency> BrowseCurrency(string query)
        {
            List<Currency> currencies = GetAllCurrencies();

            return currencies.Where(x => x.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()) 
                                      || x.ShortName.ToLowerInvariant().Contains(query.ToLowerInvariant())
                                      || x.Country.ToLowerInvariant().Contains(query.ToLowerInvariant())).ToList();
        }

       
    }
}