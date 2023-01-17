using CryptoChecker.Application.Interfaces;
using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Application.Services
{
    public class SummaryService : ISummaryService
    {
        public ISummaryService.Summary GetMarketOrderSummary(MarketOrder order, decimal assetPrice)
        {
            // order summary does not take into account whether it was a buy or a sell (same as Blockfolio app) 
            var marketValue = order.Size * assetPrice;
            var cost = (order.Size * order.FilledPrice) + order.Fee;
            if (cost == 0)
            {
                return new(0, 0, 0, 0);
            }

            var relativeChange = (marketValue / cost) - new decimal(1);
            // absolute change is the difference between the current market value of the order subtracted by order's cost
            var absoluteChange = marketValue - cost;
            return new(absoluteChange, relativeChange, marketValue, cost);
        }

        public ISummaryService.Summary GetAverageOfSummaries(IEnumerable<ISummaryService.Summary> summaries)
        {
            decimal totalMarketValue = 0, totalCost = 0, totalAbsoluteChange = 0;

            foreach (var summary in summaries)
            {
                totalMarketValue += summary.MarketValue;
                totalCost += summary.Cost;
                totalAbsoluteChange += summary.AbsoluteChange;
            }

            if (totalCost == 0)
            {
                return new ISummaryService.Summary(0, 0, 0, 0);
            }

            decimal totalRelativeChange = (totalMarketValue / totalCost) - 1m;

            return new(totalAbsoluteChange, totalRelativeChange, totalMarketValue, totalCost);
        }

        public ISummaryService.Summary GetPortfolioEntrySummary(List<MarketOrder> portfolioEntryOrders,
            decimal assetPrice)
        {
            decimal totalHoldingSize = 0, totalSellValue = 0, totalCost = 0, totalFee = 0;

            portfolioEntryOrders
                .ForEach(order =>
                {
                    // determine the holding size (negative when the order was a sell) and add it tot he sum of all holdings
                    totalHoldingSize += order.Size * (order.Buy ? 1 : -1);
                    // compute the value of the order
                    var orderValue = order.Size * order.FilledPrice;

                    if (!order.Buy)
                    {
                        // sum all value of all sell transactions
                        totalSellValue += orderValue;
                    }
                    else
                    {
                        // sum cost of all buy transactions
                        totalCost += orderValue;
                    }

                    totalFee += order.Fee;
                });

            if (totalCost == 0)
            {
                return new ISummaryService.Summary(0, 0, 0, 0);
            }

            // current total holding value is computing by multiplying the total holding size with the given price of the asset
            decimal currentTotalHoldingValue = totalHoldingSize * assetPrice;

            // use the same formula as Blockfolio app does to compute the total absolute change
            decimal totalAbsoluteChange = currentTotalHoldingValue + totalSellValue - totalCost - totalFee;
            decimal totalRelativeChange = totalAbsoluteChange / (totalCost + totalFee);

            return new ISummaryService.Summary(totalAbsoluteChange, totalRelativeChange, currentTotalHoldingValue,
                totalCost + totalFee);
        }

        // summary of a portfolio is calculated by computing the average of all entry summaries present in it
        public ISummaryService.Summary GetPortfolioSummary(List<ISummaryService.Summary> portfolioEntrySummaries) =>
            GetAverageOfSummaries(portfolioEntrySummaries);
    }
}
