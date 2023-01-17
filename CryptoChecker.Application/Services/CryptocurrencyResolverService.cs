using CryptoChecker.Application.Interfaces;
using static CryptoChecker.Domain.Entities.SourceModel;

namespace CryptoChecker.Application.Services
{
    public class CryptocurrencyResolverService : ICryptocurrencyResolver
    {
        private ICryptoStatsSource _cryptoStatsSource;

        //mapping symbols to cryptocurrencies
        private Dictionary<String, Cryptocurrency> _nameToCryptocurrencyDictionary;

        public CryptocurrencyResolverService(ICryptoStatsSource cryptoStatsSource)
        {
            _cryptoStatsSource = cryptoStatsSource;
        }

        public async Task Refresh()
        {
            _nameToCryptocurrencyDictionary = new();

            // fetch all cryptocurrencies and add them to the dictionary using the symbol as a key
            (await _cryptoStatsSource.GetAvailableCryptocurrencies())
                //workaround till Coingecko removes binance-peg entries
                .Where(c => !c.Id.Contains("binance-peg")).ToList()
                .ForEach(c => _nameToCryptocurrencyDictionary.TryAdd(c.Symbol, c));
        }

        public async Task<Cryptocurrency> Resolve(string symbol)
        {
            if (_nameToCryptocurrencyDictionary?.GetValueOrDefault(symbol) == null) 
                await Refresh();

            return _nameToCryptocurrencyDictionary.GetValueOrDefault(symbol, null);
        }
    }
}
