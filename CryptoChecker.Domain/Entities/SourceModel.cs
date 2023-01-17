using Newtonsoft.Json;

namespace CryptoChecker.Domain.Entities
{
    public class SourceModel
    {
        public record MarketEntry(string Id, string Symbol, string Name, decimal CurrentPrice, long MarketCap,
            [JsonProperty("price_change_24h", NullValueHandling = NullValueHandling.Ignore)]
        float? PriceChange24H = 0f,
            [JsonProperty("price_change_percentage_24h", NullValueHandling = NullValueHandling.Ignore)]
        float? PriceChangePercentage24H = 0f
        );

        /// <summary>
        /// A record containing a basic information about a cryptocurrency
        /// </summary>
        public record Cryptocurrency(string Id, string Symbol, string Name);
    }
}
