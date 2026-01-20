using RPGFramework.Enums;
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

        #region Indexer
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
        #endregion

        #region Add Method
        public void Add(TKey key, TValue value)
        {
            if (_items.ContainsKey(key))
            {
                throw new ArgumentException($"An item with the key '{key}' already exists in the catalog.");
            }
            _items[key] = value;
        }
        #endregion

        public bool ContainsKey(TKey key) => _items.ContainsKey(key);

        #region LoadCatalogAsync Method
        public async Task LoadCatalogAsync()
        {
            try
            {
                var loadedCatalog = await GameState.Persistence.LoadCatalogAsync<Catalog<TKey, TValue>>(Name);
                if (loadedCatalog != null)
                {
                    _items.Clear();
                    foreach (var kvp in loadedCatalog._items)
                    {
                        _items[kvp.Key] = kvp.Value;
                    }

                    GameState.Log(DebugLevel.Info, $"Catalog '{Name}' loaded with {_items.Count} items.");
                }
            }
            catch (FileNotFoundException fex)
            {
                GameState.Log(DebugLevel.Error, $"Error loading catalog '{Name}' (will use blank): {fex.Message}");               
            }
            return;
        }
        #endregion

        public bool Remove(TKey key) => _items.Remove(key);

        #region SaveCatalogAsync Method
        // Use this method to save the catalog to persistent storage
        public async Task SaveCatalogAsync()
        {
            await GameState.Persistence.SaveCatalogAsync(this, Name);
            return;
        }
        #endregion

        public bool TryGetValue(TKey key, out TValue? value) => _items.TryGetValue(key, out value);




    }
}
