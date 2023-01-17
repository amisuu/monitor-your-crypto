using static CryptoChecker.Domain.Entities.SourceModel;

namespace CryptoChecker.Application.Interfaces
{
    public interface ICryptocurrencyResolver
    {
        /// <summary>
        /// Resolves a cryptocurrency based on the given cryptocurrency symbol. Returns null value when the symbol does
        /// not map to any cryptocurrency.
        /// </summary>
        public Task<Cryptocurrency> Resolve(string symbol);

        /// <summary>
        /// Refreshes the database of cryptocurrencies
        /// </summary>
        /// <returns>A task that is finished when all cryptocurrencies are fetched and mapped to their symbols</returns>
        public Task Refresh();
    }
}
