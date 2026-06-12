using System;
using Microsoft.Extensions.Caching.Memory;

namespace TripXMLMain
{
    /// <summary>
    /// HttpApplicationState replacement: process-wide key/value state backed by the host's
    /// IMemoryCache singleton (the same instance modMain uses, so keys like ttACL and
    /// XSD{user}In/Out are shared). Entries never expire, matching Application semantics.
    /// Lives in the hub so GDS adapters can read decoding tables without referencing the host.
    /// </summary>
    public static class AppState
    {
        private static IMemoryCache _cache;

        public static void Initialize(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public static object Get(string key) => _cache?.Get(key);

        public static T Get<T>(string key) => _cache is null ? default : _cache.Get<T>(key);

        public static void Set(string key, object value) => _cache?.Set(key, value);

        public static void Remove(string key) => _cache?.Remove(key);
    }
}
