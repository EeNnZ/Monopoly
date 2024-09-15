using Microsoft.EntityFrameworkCore;
using Monopoly.API.Data.Entities;
using Monopoly.API.Services.Interfaces;

namespace Monopoly.API.Services.Boxes;

/// <summary>
/// </summary>
/// <param name="context"></param>
public class BoxesService(MainDbContext context) : IDataService<Box>
{
    /// <inheritdoc />
    public async Task<Box?> GetByIdAsync(long id)
    {
        if (context.Boxes == null)
            return null;

        return await context.Boxes.Include(b => b.Pallet).FirstOrDefaultAsync(b => b.Id == id);
    }


    /// <inheritdoc />
    public async Task<IEnumerable<Box>?> GetAllAsync()
    {
        if (context.Boxes == null)
            return Enumerable.Empty<Box>();

        var boxes = await context.Boxes.Include(b => b.Pallet).ToListAsync();

        return boxes;
    }

    /// <inheritdoc />
    public async Task<bool?> DeleteByIdAsync(long id)
    {
        if (context.Boxes == null)
            return null;

        var target = await context.Boxes.FindAsync(id);

        if (target == null)
            return true;

        context.Boxes.Remove(target);

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
    public async Task<Box?> Create(Box entity)
    {
        if (context.Boxes == null)
            return null;

        await context.Boxes.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }
}