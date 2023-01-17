using static CryptoChecker.Domain.Entities.SourceModel;

namespace CryptoChecker.Application.Interfaces
{
    public interface ICryptoStatsSource
    {
        /// <summary>
        /// Gets all market entries matching the given IDs, price values are relative to the currency specified.
        /// </summary>
        public Task<List<MarketEntry>> GetMarketEntries(string currency, params string[] ids);

        /// <summary>
        /// Gets a list of all available cryptocurrencies
        /// </summary>
        public Task<List<Cryptocurrency>> GetAvailableCryptocurrencies();
    }
}
