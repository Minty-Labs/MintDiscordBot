namespace Discord.Common.Other
{
    public class CacheManagerList<T>
    {
        private readonly List<CachedValue<T>> _cache = [];
        private readonly int _maxAgeInSeconds;

        public CacheManagerList(int maxAgeInSeconds = 3600)
        {
            _maxAgeInSeconds = maxAgeInSeconds;
        }

        public void Add(T value, int? maxAgeInSecondsOverride = null)
        {
            var cachedItem = new CachedValue<T>(value, DateTime.Now, maxAgeInSecondsOverride ?? _maxAgeInSeconds);
            _cache.Remove(cachedItem);
            _cache.Add(cachedItem);
        }

        public void MaintainCache()
        {
            var itemsToRemove = new List<CachedValue<T>>();
            foreach (var item in _cache)
            {
                if (!item.CheckValidity())
                    itemsToRemove.Add(item);
            }

            foreach (var item in itemsToRemove)
            {
                _cache.Remove(item);
            }
        }

        public bool TryGetValue(T value, out T cachedValue)
        {
            var cachedItem = new CachedValue<T>(value, DateTime.Now, _maxAgeInSeconds);

            int index = _cache.IndexOf(cachedItem);
            if (index != -1)
            {
                var item = _cache[index];
                if (item.CheckValidity())
                {
                    cachedValue = item.value;
                    return true;
                }
                else
                {
                    _cache.Remove(item);
                }
            }
            cachedValue = default!;
            return false;
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
