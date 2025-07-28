using Online.Contexts;
using Online.Models;
using Online.Interfaces;

namespace Online.Repositories
{
    public abstract class Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly MigrationContext _context;

        public Repository(MigrationContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T item)
        {
            _context.Add(item);
            await _context.SaveChangesAsync();//generate and execute the DML quries for the objects whse state is in ['added','modified','deleted'],
            return item;
        }

        public async Task<T> Delete(K key)
        {
            var item = await GetById(key);
            if (item != null)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for deleting");
        }

        public abstract Task<T> GetById(K key);
        public abstract Task<IEnumerable<T>> GetAll();
        
        public async Task<T> Update(K key, T item)
        {
            var myItem = await GetById(key);
            if (myItem != null)
            {
                _context.Entry(myItem).CurrentValues.SetValues(item);
                await _context.SaveChangesAsync();
                return item;
            }
            throw new Exception("No such item found for updation");
        }

    }
}