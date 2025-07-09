using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain;

namespace Monopoly.Warehouse.WebHost.Controllers;

[ApiController]
[Route("[controller]")]
public class LazyCacheController<T>(IRepository<T> repository,
                                    ILogger<LazyCacheController<T>> logger,
                                    IAppCache cache) 
    : ControllerBase where T : BaseEntity
{
    protected string? ClassName => typeof(MemoryCacheController<T>).FullName;
    
    protected readonly ILogger<LazyCacheController<T>> Logger = logger;
    protected readonly IAppCache Cache = cache;
    
    public async Task<T?> GetAsync(Guid id)
    {
        string key = $"getSingle{ClassName}";
        return await Cache.GetOrAddAsync(key, () => PopulateCacheSingleItem(id),
                                         DateTimeOffset.Now.AddMinutes(1));
    }
    
    public async Task<IEnumerable<T?>?> GetAllAsync(Guid id)
    {
        string key = $"getAll{ClassName}";
        return await Cache.GetOrAddAsync(key, PopulateCacheItemsCollection, DateTimeOffset.Now.AddMinutes(1));
    }

    private async Task<T?> PopulateCacheSingleItem(Guid id)
    {
        T? result = await repository.GetByIdAsync(id);
        Logger.LogInformation($"Loaded 1 item of type {typeof(T).Name} to cache");
        
        return result;
    }
    private async Task<IEnumerable<T?>> PopulateCacheItemsCollection()
    {
        ICollection<T> result = await repository.GetAllAsync();
        Logger.LogInformation($"Loaded {result.Count} items of type {typeof(T).Name} to cache");
        
        return result;
    }
}