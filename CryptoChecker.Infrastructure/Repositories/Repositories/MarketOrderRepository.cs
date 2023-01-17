using CryptoChecker.Domain.Interfaces;
using SqlKata.Execution;
using static CryptoChecker.Domain.Entities.MainEntity;

namespace CryptoChecker.Infrastructure.Repositories.Repositories
{
    public class MarketOrderRepository : SqlKataRepository<MarketOrder>, IMarketOrderRepository
    {
        // decimal precision to be used when storing order price and size (enough to support satoshis - smallest unit of bitcoin)
        private const int DecimalPrecision = 100000000;

        public MarketOrderRepository(SqlKataDatabase db) : base(db, SqlSchema.TableMarketOrders)
        {
        }

        protected override int GetEntryId(MarketOrder entry) => entry.Id;

        protected override object ToRow(MarketOrder entry)
        {
            return new
            {
                // make sure to multiply the price, fee and size with the decimal precision
                filled_price = (long)(entry.FilledPrice * DecimalPrecision),
                fee = (long)(entry.Fee * DecimalPrecision),
                size = (long)(entry.Size * DecimalPrecision),
                // date stored as unix timestamps
                date = ((DateTimeOffset)entry.Date).ToUnixTimeSeconds(),
                buy = entry.Buy ? 1 : 0,
                portfolio_entry_id = entry.PortfolioEntryId,
            };
        }

        protected override MarketOrder FromRow(dynamic d) =>
            // make sure to divide the price, fee and size by the decimal precision
            new(
                Decimal.Divide(d.filled_price, DecimalPrecision),
                Decimal.Divide(d.fee, DecimalPrecision),
                Decimal.Divide(d.size, DecimalPrecision),
                DateTimeOffset.FromUnixTimeSeconds((int)d.date).DateTime.ToLocalTime(),
                d.buy > 0,
                (int)d.id,
                (int)d.portfolio_entry_id
            );

        public List<MarketOrder> GetAllByPortfolioEntryId(int portfolioEntryId) =>
            RowsToObjects(Db.Get().Query(TableName).Where(SqlSchema.MarketOrdersPortfolioEntryId, portfolioEntryId)
                .Get());

        public int DeletePortfolioEntryOrders(int portfolioEntryId) =>
            Db.Get().Query(TableName).Where(SqlSchema.MarketOrdersPortfolioEntryId, portfolioEntryId).Delete();
    }
}
