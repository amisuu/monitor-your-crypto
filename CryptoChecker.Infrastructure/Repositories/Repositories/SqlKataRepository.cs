using CryptoChecker.Domain.Interfaces;
using SqlKata.Execution;

namespace CryptoChecker.Infrastructure.Repositories.Repositories
{
    public abstract class SqlKataRepository<T> : IRepository<T>
    {
        protected readonly SqlKataDatabase Db;
        protected readonly string TableName;

        public SqlKataRepository(SqlKataDatabase db, string tableName)
        {
            Db = db;
            TableName = tableName;
        }

        /// <summary>
        /// Converts a object of type T to a generic untyped object (to a table row)
        /// </summary>
        /// <param name="entry">An object of type T to be converted</param>
        /// <returns>Generic object (table row) representing the given object of type T</returns>
        protected abstract object ToRow(T entry);

        /// <summary>
        /// Converts a generic object (table row) to an object of type T
        /// </summary>
        /// <param name="d">Object to be converted to an object of type T</param>
        /// <returns>Returns an object of type T converted from the generic object (table row)</returns>
        protected abstract T FromRow(dynamic d);

        /// <summary>
        /// Returns an ID of the given object of type T
        /// </summary>
        /// <param name="entry">Object whose ID should be returner</param>
        /// <returns>ID of the given object</returns>
        protected abstract int GetEntryId(T entry);

        /// <summary>
        /// Returns the name of the primary key column
        /// </summary>
        protected string GetPrimaryKeyColumnName => SqlSchema.TableIdPrimaryKey;

        public int Add(T entry) => Db.Get().Query(TableName).InsertGetId<int>(ToRow(entry));

        public bool Update(T entry) =>
            Db.Get().Query(TableName).Where(GetPrimaryKeyColumnName, GetEntryId(entry)).Update(ToRow(entry)) > 0;

        public bool Delete(T entry) => Db.Get().Query(TableName).Where(GetPrimaryKeyColumnName, GetEntryId(entry)).Delete() > 0;

        public T Get(int id)
        {
            var result = Db.Get().Query(TableName).Where(GetPrimaryKeyColumnName, id).FirstOrDefault();

            // convert the given table row to an object of type T
            return result == null ? default : FromRow(result);
        }

        // select all rows from the database and converts them to objects of type T
        public List<T> GetAll() => RowsToObjects(Db.Get().Query(TableName).Select().Get());

        // converts a collection of table rows to a list of objects of type T
        protected List<T> RowsToObjects(IEnumerable<dynamic> rows) => rows.Select(row => (T)FromRow(row)).ToList();

    }
}
