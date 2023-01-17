namespace CryptoChecker.Domain.Interfaces
{
    public interface IRepository<T>
    {
        public int Add(T entry);
        public T Get(int id);
        public List<T> GetAll();
        public bool Update(T entry);
        public bool Delete(T entry);
    }
}
