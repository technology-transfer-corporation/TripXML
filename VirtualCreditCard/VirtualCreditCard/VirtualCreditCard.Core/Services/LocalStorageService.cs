using Microsoft.Extensions.Caching.Memory;
using VirtualCreditCard.Core.Service.Interfaces;

namespace VirtualCreditCard.Core.Service
{
    public class LocalStorageService : IStorageService
    {
        public IMemoryCache _memoryCache { get; }

        public LocalStorageService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Set<T>(string key, T value)
        {
            _memoryCache.Set(key, value);
        }

        public T? TryGet<T>(string key) where T : class
        {
            return _memoryCache.TryGetValue(key, out T? cacheEntry) ? cacheEntry : null;
        }
    }
}
