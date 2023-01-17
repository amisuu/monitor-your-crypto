using CryptoChecker.Domain.Interfaces;
using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Application.Services
{
    public interface IPortfolioService
    {
        Portfolio CreatePortfolio(string name, string description, Currency currency);
        bool DeletePortfolio(Portfolio portfolio);
        bool UpdatePortfolio(Portfolio portfolio);
        Portfolio GetPortfolio(int id);
        List<Portfolio> GetPortfolios();
    }

    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IPortfolioEntryService _portfolioEntryService;

        public PortfolioService(IPortfolioRepository portfolioRepository,
            IPortfolioEntryService portfolioEntryService)
        {
            _portfolioRepository = portfolioRepository;
            _portfolioEntryService = portfolioEntryService;
        }

        public Portfolio CreatePortfolio(string name, string description, Currency currency)
        {
            var portfolio = new Portfolio(name, description, currency);
            return portfolio with { Id = _portfolioRepository.Add(portfolio)};
        }

        public bool DeletePortfolio(Portfolio portfolio)
        {
            _portfolioEntryService.DeletePortfolioEntries(portfolio.Id);
            return _portfolioRepository.Delete(portfolio);
        }

        public bool UpdatePortfolio(Portfolio portfolio) => _portfolioRepository.Update(portfolio);

        public Portfolio GetPortfolio(int id) => _portfolioRepository.Get(id);

        public List<Portfolio> GetPortfolios() => _portfolioRepository.GetAll();

    }
}
