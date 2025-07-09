using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain;

namespace Monopoly.Warehouse.DataAccess.Repositories;

public class InMemoryRepository<T>(List<T>? entities) : IRepository<T>
    where T : BaseEntity
{
    private ICollection<T> Data { get; } = entities ?? new List<T>();


    public Task<ICollection<T>> GetAllAsync()
    {
        return Task.FromResult(Data);
    }

    public Task<T?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
    }

    public Task<Guid> CreateAsync(T entity)
    {
        Data.Add(entity);
        return Task.FromResult(entity.Id);
    }

    public Task CreateManyAsync(ICollection<T> entities)
    {
        foreach (T entity in entities)
        {
            Data.Add(entity);
        }
        
        return Task.CompletedTask;
    }

    public Task<T?> UpdateAsync(T entity)
    {
        if (Data.All(x => x.Id != entity.Id))
            throw new Exception($"Entity {entity.Id} does not exist");

        DeleteAsync(entity);
        CreateAsync(entity);

        return Task.FromResult<T?>(entity);
    }

    public Task DeleteAsync(T entity)
    {
        Data.Remove(entity);
        return Task.CompletedTask;
    }
}