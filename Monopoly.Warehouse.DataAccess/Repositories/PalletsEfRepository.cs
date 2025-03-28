using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Monopoly.Warehouse.Core.Abstractions.Repositories;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.DataAccess.Repositories;

public class PalletsEfRepository : EfRepository<Pallet>, IPalletsRepository
{
    protected PalletsEfRepository(DbContext context) : base(context)
    {
    }

    public async Task<Pallet?> GetPalletAsync(Expression<Func<Pallet, bool>> predicate)
    {
        return await DbSet.AsNoTracking().SingleOrDefaultAsync(predicate);
    }

    public async Task<ICollection<Pallet>> GetPalletsWithBoxesAsync()
    {
        return await DbSet.Where(pallet => pallet.Boxes.Any()).ToListAsync();
    }
}