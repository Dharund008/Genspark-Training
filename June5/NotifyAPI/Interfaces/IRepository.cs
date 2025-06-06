namespace NotifyAPI.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        public Task<T> Add(T item);
        public Task<T> Get(K key);
    }
}