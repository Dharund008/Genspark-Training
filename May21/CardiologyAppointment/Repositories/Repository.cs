using CardiologyAppointmentApp.Interfaces;

namespace CardiologyAppointmentApp.Repositories
{
    public abstract class Repository<K, T> : IRepositor<K, T> where T : class
    {
        protected List<T> _items = new List<T>();
        protected abstract K GenerateID();
        public abstract ICollection<T> GetAll();
        public abstract T GetById(K id);

        public T Add(T item)
        {
            var id = GenerateID();
            var prop = typeof(T).GetProperty("Id");
            if (prop != null)
                prop.SetValue(item, id);

            _items.Add(item);
            return item;
        }

        public T Delete(K id)
        {
            var item = GetById(id);
            _items.Remove(item);
            return item;
        }

        public T Update(T item)
        {
            var id = (K)item.GetType().GetProperty("Id")!.GetValue(item);
            var existing = GetById(id);
            var index = _items.IndexOf(existing);
            _items[index] = item;
            return item;
        }
    }
}
