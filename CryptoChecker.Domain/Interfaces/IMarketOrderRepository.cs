using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Domain.Interfaces
{
    public interface IMarketOrderRepository : IRepository<MarketOrder>
    {
        public List<MarketOrder> GetAllByPortfolioEntryId(int portfolioEntryId);
        public int DeletePortfolioEntryOrders(int portfolioEntryId);
    }
}
