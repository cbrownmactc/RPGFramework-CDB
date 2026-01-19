using RPGFramework.Interfaces;

namespace RPGFramework
{
    internal class Catalog<TKey, TValue> : ICatalog<TKey, TValue>
        where TKey : notnull
    {
        #region --- Properties ---
        public IEnumerable<TKey> Keys => _items.Keys;
        public IEnumerable<TValue> Values => _items.Values;
        public int Count => _items.Count;
        public string Name => typeof(TValue).Name + "Catalog";
        #endregion

        // Fields
        private readonly Dictionary<TKey, TValue> _items = [];

        public Catalog()
        {
        }

        // Creates the indexer to access items by key
        public TValue this[TKey key]
        {
            get
            {
                if (!_items.TryGetValue(key, out var value))
                {
                    throw new KeyNotFoundException($"The key '{key}' was not found in the catalog.");
                }
                return value;
            }
            set
            {
                _items[key] = value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (_items.ContainsKey(key))
            {
                throw new ArgumentException($"An item with the key '{key}' already exists in the catalog.");
            }
            _items[key] = value;
        }

        public bool ContainsKey(TKey key) => _items.ContainsKey(key);
        public bool Remove(TKey key) => _items.Remove(key);

        // Use this method to save the catalog to persistent storage
        public async Task SaveCatalog()
        {
            await GameState.Persistence.SaveCatalog(this, Name);
            return;
        }

        public bool TryGetValue(TKey key, out TValue? value) => _items.TryGetValue(key, out value);




    }
}
