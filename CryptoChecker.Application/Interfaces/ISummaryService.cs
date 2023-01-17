using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Application.Interfaces
{
    public interface ISummaryService
    {
        /// <param name="AbsoluteChange">
        /// Absolute change of the of the tracked entity (typically in a USD, EUR,...) 
        /// </param>
        /// <param name="RelativeChange">
        /// Relative change of the of the tracked entity (0 meaning change whatsoever, 1.0 meaning 100% increase in market value,
        /// -1.0) meaning -100% decrease in market value 
        /// </param>
        /// <param name="MarketValue">
        /// Total market value of the tracked entity (entity size multiplied by the current entity value) 
        /// </param>
        /// <param name="Cost">
        /// Cost of the tracked entity (entity size multiplied by the price of the entity at the time it was traded)
        /// </param>
        public record Summary(decimal AbsoluteChange, decimal RelativeChange, decimal MarketValue, decimal Cost);

        /// <summary>
        /// Calculates a summary of the given market order
        /// </summary>
        public Summary GetMarketOrderSummary(MarketOrder order, decimal assetPrice);

        /// <summary>
        /// Calculates a summary of the given list of orders of a portfolio entry
        /// </summary>
        public Summary GetPortfolioEntrySummary(List<MarketOrder> portfolioEntryOrders, decimal assetPrice);

        /// <summary>
        /// Calculates a summary of the given portfolio entry summaries  
        /// </summary>
        public Summary GetPortfolioSummary(List<Summary> portfolioEntrySummaries);
    }
}
