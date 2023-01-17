using CryptoChecker.Domain.Interfaces;
using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Infrastructure.Repositories.Repositories
{
    public class PortfolioRepository : SqlKataRepository<Portfolio>, IPortfolioRepository
    {
        public PortfolioRepository(SqlKataDatabase db) : base(db, SqlSchema.TablePortfolios)
        {
        }

        protected override int GetEntryId(Portfolio entry) => entry.Id;

        protected override object ToRow(Portfolio entry) => new
        {
            name = entry.Name,
            description = entry.Description,
            currency_code = (int)entry.Currency
        };

        protected override Portfolio FromRow(dynamic d) =>
            new((string)d.name, (string)d.description, (Currency)d.currency_code, (int)d.id);
    }
}
