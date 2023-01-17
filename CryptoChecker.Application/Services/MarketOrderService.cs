using CryptoChecker.Domain.Interfaces;
using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Application.Services
{
    public interface IMarketOrderService
    {
        MarketOrder CreateMarketOrder(decimal filledPrice, decimal fee, decimal size,
            DateTime date, bool buy, int portfolioEntryId);
        bool DeleteMarketOrder(MarketOrder order);
        bool UpdateMarketOrder(MarketOrder order);
        MarketOrder GetMarketOrder(int id);
        List<MarketOrder> GetPortfolioEntryOrders(int portfolioEntryId);
        int DeletePortfolioEntryOrders(int portfolioEntryId);
    }

    public class MarketOrderService : IMarketOrderService
    {
        private readonly IMarketOrderRepository _marketOrderRepository;

        public MarketOrderService(IMarketOrderRepository marketOrderRepository)
        {
            _marketOrderRepository = marketOrderRepository;
        }

        public MarketOrder CreateMarketOrder(decimal filledPrice, decimal fee, decimal size, DateTime date, bool buy,
            int portfolioEntryId)
        {
            var marketOrder = new MarketOrder(filledPrice, fee, size, date, buy, PortfolioEntryId: portfolioEntryId);
            return marketOrder with { Id = _marketOrderRepository.Add(marketOrder)};
        }

        public bool DeleteMarketOrder(MarketOrder order) => _marketOrderRepository.Delete(order);

        public bool UpdateMarketOrder(MarketOrder order) => _marketOrderRepository.Update(order);

        public MarketOrder GetMarketOrder(int id) => _marketOrderRepository.Get(id);

        public List<MarketOrder> GetPortfolioEntryOrders(int portfolioEntryId) =>
            _marketOrderRepository.GetAllByPortfolioEntryId(portfolioEntryId);

        public int DeletePortfolioEntryOrders(int portfolioEntryId) =>
            _marketOrderRepository.DeletePortfolioEntryOrders(portfolioEntryId);
    }
}
