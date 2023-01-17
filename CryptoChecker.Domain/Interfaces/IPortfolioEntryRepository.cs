using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Domain.Interfaces
{
    public interface IPortfolioEntryRepository : IRepository<PortfolioEntry>
    {
        public List<PortfolioEntry> GetAllByPortfolioId(int portfolioId);
        public int DeletePortfolioEntries(int portfolioId);
    }
}
