using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain;

namespace Monopoly.Warehouse.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class DistributedCacheController<T>(IRepository<T> repository,
                                           ILogger<LazyCacheController<T>> logger,
                                           IDistributedCache cache) 
    : ControllerBase where T : BaseEntity
{
    protected string? ClassName => typeof(MemoryCacheController<T>).FullName;
    
    protected readonly ILogger<LazyCacheController<T>> Logger = logger;
    protected readonly IDistributedCache Cache = cache;
    
    public async Task<T?> GetAsync(Guid id)
    {
        string key = $"getSingle{ClassName}";

        byte[]? resultBytes = await Cache.GetAsync(key);

        if (resultBytes != null)
        {
            Logger.LogInformation($"Get {typeof(T).Name} from cache");
            return JsonSerializer.Deserialize<T>(resultBytes);
        }

        T? result = await repository.GetByIdAsync(id);

        var cacheExpirationOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        };
        
        resultBytes = JsonSerializer.SerializeToUtf8Bytes(result);
        await Cache.SetAsync(key, resultBytes, cacheExpirationOptions);
        
        Logger.LogInformation($"Loaded 1 item of type {typeof(T).Name} to cache");

        return result;
    }
    
    public async Task<IEnumerable<T?>?> GetAllAsync(Guid id)
    {
        string key = $"getSingle{ClassName}";

        byte[]? resultBytes = await Cache.GetAsync(key);

        if (resultBytes != null)
        {
            Logger.LogInformation($"Get {typeof(T).Name} from cache");
            return JsonSerializer.Deserialize<IEnumerable<T>>(resultBytes);
        }

        ICollection<T> result = await repository.GetAllAsync();

        var cacheExpirationOptions = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        };
        
        resultBytes = JsonSerializer.SerializeToUtf8Bytes(result);
        await Cache.SetAsync(key, resultBytes, cacheExpirationOptions);
        
        Logger.LogInformation($"Loaded 1 item of type {typeof(T).Name} to cache");

        return result;
    }
}