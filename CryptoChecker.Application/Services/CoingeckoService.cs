using CryptoChecker.Application.Interfaces;
using Tiny.RestClient;
using static CryptoChecker.Domain.Entities.SourceModel;

namespace CryptoChecker.Application.Services
{
    public class CoingeckoService : ICryptoStatsSource
    {
        private const string BaseUrl = "https://api.coingecko.com/api/";
        private const string ApiVersion = "v3";
        private readonly TinyRestClient _client = new(new HttpClient(), $"{BaseUrl}{ApiVersion}/");

        public CoingeckoService()
        {
            //set JSON attribute name
            _client.Settings.Formatters.OfType<JsonFormatter>().First().UseSnakeCase();
        }

        //coins/markets API endpoint
        public async Task<List<MarketEntry>> GetMarketEntries(string currency, params string[] ids) => await _client
                .GetRequest("coins/markets").AddQueryParameter("vs_currency", currency)
                .AddQueryParameter("ids", String.Join(",", ids)).ExecuteAsync<List<MarketEntry>>();

        //coins/list API endpoint
        public async Task<List<Cryptocurrency>> GetAvailableCryptocurrencies() => await _client
                .GetRequest("coins/list").ExecuteAsync<List<Cryptocurrency>>();
    }
}
