namespace Discord.Common.Other
{
    public class CacheManagerDictionary<TIndex, TValue>
    {
        private readonly Dictionary<TIndex, CachedValue<TValue>> _cache = [];
        private readonly int _maxAgeInSeconds;

        public CacheManagerDictionary(int maxAgeInSeconds = 3600)
        {
            _maxAgeInSeconds = maxAgeInSeconds;
        }

        public void Add(TIndex index, TValue value, int? maxAgeInSecondsOverride = null)
        {
            _cache[index] = new CachedValue<TValue>(value, DateTime.Now, maxAgeInSecondsOverride ?? _maxAgeInSeconds);
        }

        public void MaintainCache()
        {
            var keysToRemove = new List<TIndex>();
            foreach (var (key, value) in _cache)
            {
                if (!value.CheckValidity())
                    keysToRemove.Add(key);
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }

        public bool TryGetValue(TIndex index, out TValue value)
        {
            if (_cache.TryGetValue(index, out var cachedValue))
            {
                if (cachedValue.CheckValidity())
                {
                    value = cachedValue.value;
                    return true;
                }
                else
                {
                    _cache.Remove(index);
                }
            }
            value = default!;
            return false;
        }

        public void Clear()
        {
            _cache.Clear();
        }
    }
}
