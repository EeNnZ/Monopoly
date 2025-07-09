using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain;

namespace Monopoly.Warehouse.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class MemoryCacheController<T>(IRepository<T> repository,
                                      IMemoryCache memoryCache, 
                                      ILogger<MemoryCacheController<T>> logger) 
    : ControllerBase where T : BaseEntity
{
    protected string? ClassName => typeof(MemoryCacheController<T>).FullName;
    
    protected readonly ILogger<MemoryCacheController<T>> Logger = logger;
    protected readonly IMemoryCache MemoryCache = memoryCache;
    protected readonly SemaphoreSlim CacheSemaphore = new(1);

    public async Task<T?> GetAsync(Guid id)
    {
        string cacheKey = $"getSingle{ClassName}";

        if (MemoryCache.TryGetValue(cacheKey, out T? result))
        {
            Logger.LogInformation($"Return {typeof(T).Name} from cache");
            return result;
        }
        
        await CacheSemaphore.WaitAsync();

        try
        {
            if (MemoryCache.TryGetValue(cacheKey, out result))
            {
                Logger.LogInformation($"Return {typeof(T).Name} from cache");
                return result;
            }

            result = await repository.GetByIdAsync(id);

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                Priority = CacheItemPriority.Normal
            };
            MemoryCache.Set(cacheKey, result, cacheEntryOptions);
            
            Logger.LogInformation($"Loaded 1 item of type {ClassName} to cache");
            return result;
        }
        finally
        {
            CacheSemaphore.Release();
        }
    }

    public async Task<IEnumerable<T>?> GetAllAsync(Guid id)
    {
        string cacheKey = $"getAll{ClassName}";

        if (MemoryCache.TryGetValue(cacheKey, out IEnumerable<T>? result))
        {
            Logger.LogInformation($"Return {typeof(T).Name} from cache");
            return result;
        }
        
        await CacheSemaphore.WaitAsync();

        try
        {
            if (MemoryCache.TryGetValue(cacheKey, out result))
            {
                Logger.LogInformation($"Return {typeof(T).Name} from cache");
                return result;
            }

            result = await repository.GetAllAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                Priority = CacheItemPriority.Normal
            };
            
            MemoryCache.Set(cacheKey, result, cacheEntryOptions);
            Logger.LogInformation($"Loaded {result.Count()} item of type {ClassName} to cache");
            
            return result;
        }
        finally
        {
            CacheSemaphore.Release();
        }
    }
}