using Monopoly.Warehouse.Core.Domain;

namespace Monopoly.Warehouse.Core.Abstractions.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?>             GetByIdAsync(Guid              id);
    Task<Guid>           CreateAsync(T                  entity);
    Task                 CreateManyAsync(ICollection<T> entities);
    Task<T?>               UpdateAsync(T                  entity);
    Task                 DeleteAsync(T                  entity);
}