using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.DataAccess.Repositories;

public class BoxesEfRepository : EfRepository<Box>, IBoxesRepository
{
    public BoxesEfRepository(DbContext context) : base(context)
    {
    }

    public async Task<ICollection<Box>> GetBoxesInsidePallet(Guid palletId)
    {
        return await DbSet.Where(box => box.PalletId == palletId).Include(b => b.Pallet).ToListAsync();
    }

    public async Task<ICollection<Box>> GetAllAsync(Expression<Func<Box, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }
}