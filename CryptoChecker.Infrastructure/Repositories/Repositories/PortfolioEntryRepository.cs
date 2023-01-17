using CryptoChecker.Domain.Interfaces;
using SqlKata.Execution;
using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Infrastructure.Repositories.Repositories
{
    public class PortfolioEntryRepository : SqlKataRepository<PortfolioEntry>, IPortfolioEntryRepository
    {
        public PortfolioEntryRepository(SqlKataDatabase db) : base(db, SqlSchema.TablePortfolioEntries)
        {
        }

        protected override int GetEntryId(PortfolioEntry entry) => entry.Id;

        protected override object ToRow(PortfolioEntry entry) => new
        {
            symbol = entry.Symbol,
            portfolio_id = entry.PortfolioId
        };

        protected override PortfolioEntry FromRow(dynamic d) => new((string)d.symbol, (int)d.portfolio_id, (int)d.id);

        public List<PortfolioEntry> GetAllByPortfolioId(int portfolioId) =>
            RowsToObjects(Db.Get().Query(TableName).Where(SqlSchema.PortfolioEntriesPortfolioId, portfolioId).Get());

        public int DeletePortfolioEntries(int portfolioId) =>
            Db.Get().Query(TableName).Where(SqlSchema.PortfolioEntriesPortfolioId, portfolioId).Delete();
    }
}
