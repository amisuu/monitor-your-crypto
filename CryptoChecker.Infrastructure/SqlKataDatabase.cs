using SqlKata.Compilers;
using SqlKata.Execution;
using System.Data;

namespace CryptoChecker.Infrastructure
{
    public class SqlKataDatabase : IDisposable
    {
        private readonly QueryFactory _queryFactory;
        private readonly IDbConnection _dbConnection;

        public SqlKataDatabase(IDbConnection dbConnection, Compiler compiler)
        {
            _dbConnection = dbConnection;
            _queryFactory = new(dbConnection, compiler);

            SqlSchema.Init(this);
        }

        public QueryFactory Get() => _queryFactory;

        public void Dispose()
        {
            _dbConnection.Close();
        }
    }
}
