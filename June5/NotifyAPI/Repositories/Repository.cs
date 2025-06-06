using NotifyAPI.Contexts;
using NotifyAPI.Interfaces;

//making the class as abstract so that it can be inherited by other class to implement their respective methods

namespace NotifyAPI.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly ManagementContext _context;

        public Repository(ManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<T> Add(T item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public abstract Task<T> Get(K key);
    }
}
