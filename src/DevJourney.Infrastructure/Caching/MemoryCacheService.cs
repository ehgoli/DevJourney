using DevJourney.Application.Interfaces.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace DevJourney.Infrastructure.Caching;

public sealed class MemoryCacheService(
    IMemoryCache memoryCache,
    IOptions<CacheOptions> options) : ICacheService
{
    private readonly CacheOptions _options = options.Value;

    private readonly Dictionary<string, HashSet<string>> _tagKeys = [];
    private readonly Dictionary<string, HashSet<string>> _keyTags = [];
    private readonly Lock _tagLock = new();

    public ValueTask<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        cancellationToken.ThrowIfCancellationRequested();

        if (memoryCache.TryGetValue(key, out var value))
            return ValueTask.FromResult((T?)value);

        RemoveTagMappings(key);

        return ValueTask.FromResult<T?>(default);
    }

    public async ValueTask<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, ValueTask<T>> factory,
        TimeSpan? expiration = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        ArgumentNullException.ThrowIfNull(factory);

        cancellationToken.ThrowIfCancellationRequested();

        var cachedValue = await GetAsync<T>(
            key,
            cancellationToken);

        if (cachedValue is not null)
            return cachedValue;

        var value = await factory(cancellationToken);

        await SetAsync(
            key: key,
            value: value,
            expiration: expiration,
            tags: tags,
            cancellationToken: cancellationToken);

        return value;
    }

    public ValueTask SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        cancellationToken.ThrowIfCancellationRequested();

        var cacheExpiration =
            expiration ?? _options.DefaultExpiration;

        if (cacheExpiration <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expiration),
                "Expiration must be greater than zero.");
        }

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheExpiration
        };

        RemoveTagMappings(key);

        memoryCache.Set(
            key,
            value,
            cacheOptions);

        AddTagMappings(
            key,
            tags);

        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        cancellationToken.ThrowIfCancellationRequested();

        memoryCache.Remove(key);
        RemoveTagMappings(key);

        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveByTagAsync(
        string tag,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        cancellationToken.ThrowIfCancellationRequested();

        string[] keys;

        lock (_tagLock)
        {
            if (!_tagKeys.TryGetValue(tag, out var tagKeySet))
                return ValueTask.CompletedTask;

            keys = tagKeySet.ToArray();
        }

        foreach (var key in keys)
        {
            cancellationToken.ThrowIfCancellationRequested();

            memoryCache.Remove(key);
            RemoveTagMappings(key);
        }

        return ValueTask.CompletedTask;
    }

    private void AddTagMappings(
        string key,
        IEnumerable<string>? tags)
    {
        if (tags is null)
            return;

        var normalizedTags = tags
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (normalizedTags.Length == 0)
            return;

        lock (_tagLock)
        {
            if (!_keyTags.TryGetValue(key, out var keyTagSet))
            {
                keyTagSet = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

                _keyTags[key] = keyTagSet;
            }

            foreach (var tag in normalizedTags)
            {
                keyTagSet.Add(tag);

                if (!_tagKeys.TryGetValue(tag, out var tagKeySet))
                {
                    tagKeySet = new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                    _tagKeys[tag] = tagKeySet;
                }

                tagKeySet.Add(key);
            }
        }
    }

    private void RemoveTagMappings(string key)
    {
        lock (_tagLock)
        {
            if (!_keyTags.TryGetValue(key, out var keyTagSet))
                return;

            foreach (var tag in keyTagSet)
            {
                if (!_tagKeys.TryGetValue(tag, out var tagKeySet))
                    continue;

                tagKeySet.Remove(key);

                if (tagKeySet.Count == 0)
                    _tagKeys.Remove(tag);
            }

            _keyTags.Remove(key);
        }
    }
}