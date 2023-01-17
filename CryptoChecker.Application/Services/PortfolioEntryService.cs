using CryptoChecker.Domain.Interfaces;
using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Application.Services
{
    public interface IPortfolioEntryService
    {
        PortfolioEntry CreatePortfolioEntry(string symbol, int portfolioId);
        bool DeletePortfolioEntry(PortfolioEntry entry);
        int DeletePortfolioEntries(int portfolioId);
        bool UpdatePortfolioEntry(PortfolioEntry entry);
        PortfolioEntry GetPortfolioEntry(int id);
        List<PortfolioEntry> GetPortfolioEntries(int portfolioId);
    }

    public class PortfolioEntryService : IPortfolioEntryService
    {
        private readonly IPortfolioEntryRepository _portfolioEntryRepository;
        private readonly IMarketOrderService _marketOrderService;

        public PortfolioEntryService(IPortfolioEntryRepository portfolioEntryRepository, IMarketOrderService marketOrderService)
        {
            _portfolioEntryRepository = portfolioEntryRepository;
            _marketOrderService = marketOrderService;
        }

        public PortfolioEntry CreatePortfolioEntry(string symbol, int portfolioId)
        {
            var portfolioEntry = new PortfolioEntry(symbol, portfolioId);
            return portfolioEntry with { Id = _portfolioEntryRepository.Add(portfolioEntry) };
        }

        public bool DeletePortfolioEntry(PortfolioEntry entry)
        {
            _marketOrderService.DeletePortfolioEntryOrders(entry.Id);
            return _portfolioEntryRepository.Delete(entry);
        }

        public bool UpdatePortfolioEntry(PortfolioEntry entry) => _portfolioEntryRepository.Update(entry);

        public PortfolioEntry GetPortfolioEntry(int id) => _portfolioEntryRepository.Get(id);

        public List<PortfolioEntry> GetPortfolioEntries(int portfolioId) => _portfolioEntryRepository.GetAllByPortfolioId(portfolioId);

        public int DeletePortfolioEntries(int portfolioId)
        {
            foreach (var portfolioEntry in GetPortfolioEntries(portfolioId))
            {
                _marketOrderService.DeletePortfolioEntryOrders(portfolioEntry.Id);
            }

            return _portfolioEntryRepository.DeletePortfolioEntries(portfolioId);
        }
    }
}
