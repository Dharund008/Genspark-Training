namespace Online.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> AddAsync(T item);
        Task<T> GetByIdAsync(K key);
        Task<IEnumerable<T>> GetAllAsync();
        public Task<T> Update(K key, T item);
        public Task<T> Delete(K key);
    }
}