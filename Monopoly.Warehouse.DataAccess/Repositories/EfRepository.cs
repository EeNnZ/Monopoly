using Microsoft.EntityFrameworkCore;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain;

namespace Monopoly.Warehouse.DataAccess.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected DbContext Context { get; }
    protected DbSet<T> DbSet { get; }

    protected EfRepository(DbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }
    
    public async Task<ICollection<T>> GetAllAsync()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id); 
    }

    public async Task<Guid> CreateAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await SaveChanges();

        return entity.Id;
    }

    public async Task CreateManyAsync(ICollection<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
        
        await SaveChanges();
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        await SaveChanges();
        
        return await DbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
    }

    public async Task DeleteAsync(T entity)
    {
        T? entityEntry = await DbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);

        if (entityEntry != null)
        {
            DbSet.Remove(entityEntry);
            await SaveChanges();
        }
    }
    
    private async Task SaveChanges() => await Context.SaveChangesAsync();
}