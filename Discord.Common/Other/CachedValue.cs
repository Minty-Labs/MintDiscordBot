#pragma warning disable CA2231 // Overload operator equals on overriding value type Equals

namespace Discord.Common.Other
{
    public readonly struct CachedValue<T>
    {
        public override readonly bool Equals(object obj)
            => obj is CachedValue<T> cachedValue && value.Equals(cachedValue.value);

        public override readonly int GetHashCode()
            => value.GetHashCode();

        public readonly T value;
        public readonly DateTime creationTime;
        public readonly int maxAgeInSeconds;

        public CachedValue(T value, DateTime creationTime, int maxAgeInSeconds = 3600)
        {
            this.value = value;
            this.creationTime = creationTime;
            this.maxAgeInSeconds = maxAgeInSeconds;
        }

        public readonly bool CheckValidity()
            => !((DateTime.Now - creationTime).TotalSeconds > maxAgeInSeconds);
    }
}
