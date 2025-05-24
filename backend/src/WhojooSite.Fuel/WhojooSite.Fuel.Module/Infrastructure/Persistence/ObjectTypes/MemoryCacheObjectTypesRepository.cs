using Microsoft.Extensions.Caching.Memory;

using WhojooSite.Fuel.Module.Application.Interfaces;
using WhojooSite.Fuel.Module.Domain.TrackableObjects;

namespace WhojooSite.Fuel.Module.Infrastructure.Persistence.ObjectTypes;

internal class MemoryCacheObjectTypesRepository(IMemoryCache memoryCache, IObjectTypeRepository repository) : IObjectTypeRepository
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly IObjectTypeRepository _repository = repository;

    public async Task<ObjectTypeId?> GetIdFromNameAsync(string name, CancellationToken cancellationToken)
    {
        const string keyPrefix = nameof(GetIdFromNameAsync);
        var key = $"{keyPrefix}:{name}";

        if (_memoryCache.TryGetValue(key, out ObjectTypeId objectTypeId))
        {
            return objectTypeId;
        }

        var retrievedId = await _repository.GetIdFromNameAsync(name, cancellationToken);

        if (!retrievedId.HasValue)
        {
            return retrievedId;
        }

        var memoryCreateOptions = new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) };

        _memoryCache.Set(key, retrievedId.Value, memoryCreateOptions);

        return retrievedId;
    }
}