using Microsoft.EntityFrameworkCore;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;

namespace Monopoly.API.Services.Pallets;

/// <summary>
/// </summary>
/// <param name="context"></param>
public class PalletsService(MainDbContext context) : IDataService<Pallet>
{
    /// <inheritdoc />
    public async Task<Pallet?> GetByIdAsync(long id)
    {
        if (context.Pallets == null)
            return null;

        return await context.Pallets.Include(p => p.Boxes).FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Pallet>?> GetAllAsync()
    {
        if (context.Pallets == null)
            return Enumerable.Empty<Pallet>();

        var pallets = await context.Pallets
                                   .Include(p => p.Boxes)
                                   .ToListAsync();

        return pallets;
    }

    /// <inheritdoc />
    public async Task<bool?> DeleteByIdAsync(long id)
    {
        if (context.Pallets == null)
            return null;

        var target = await context.Pallets.FindAsync(id);

        if (target == null)
            return true;

        context.Pallets.Remove(target);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public async Task<Pallet?> Create(Pallet entity)
    {
        if (context.Pallets == null)
            return null;

        await context.Pallets.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }
}