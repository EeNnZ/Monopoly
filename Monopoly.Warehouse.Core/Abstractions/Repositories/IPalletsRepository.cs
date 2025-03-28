using System.Linq.Expressions;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.Core.Abstractions.Repositories;

public interface IPalletsRepository : IRepository<Pallet>
{
    Task<Pallet?> GetPalletAsync(Expression<Func<Pallet, bool>> predicate);
    Task<ICollection<Pallet>> GetPalletsWithBoxesAsync();
}