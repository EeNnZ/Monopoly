using System.Linq.Expressions;
using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.Core.Abstractions.Repositories;

public interface IBoxesRepository : IRepository<Box>
{
    Task<ICollection<Box>> GetBoxesInsidePallet(Guid palletId);
    Task<ICollection<Box>> GetAllAsync(Expression<Func<Box, bool>> predicate);
}