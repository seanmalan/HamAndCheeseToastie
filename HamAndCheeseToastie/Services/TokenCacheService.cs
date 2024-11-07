using System;
using Microsoft.Extensions.Caching.Memory;

public class TokenCacheService
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _expiryTime = TimeSpan.FromMinutes(30);

    public TokenCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void CacheToken(string token, string userId)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _expiryTime
        };
        _cache.Set(token, userId, cacheEntryOptions);
    }

    public bool IsTokenValid(string token)
    {
        return _cache.TryGetValue(token, out _);
    }

    public void RemoveToken(string token)
    {
        _cache.Remove(token);
    }
}
