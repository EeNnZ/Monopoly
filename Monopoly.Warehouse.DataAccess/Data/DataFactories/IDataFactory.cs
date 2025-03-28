using Monopoly.Warehouse.Core.Domain.Warehouse.Entities;

namespace Monopoly.Warehouse.DataAccess.Data.DataFactories;

/// <summary>
///     An interface for generating a specified number of objects of type T.
///     This interface is designed to be implemented by classes that provide
///     functionality to create instances of objects that derive from
///     <see cref="WarehouseObject" />.
/// </summary>
/// <typeparam name="T">The type of objects to generate, which must inherit from <see cref="WarehouseObject" />.</typeparam>
public interface IDataFactory<T> where T : WarehouseObject
{
    /// <summary>
    ///     Asynchronously generates a collection of objects of type T.
    /// </summary>
    /// <param name="count">The number of objects to generate.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task's result
    ///     contains an enumerable collection of generated objects of type T.
    /// </returns>
    Task<IEnumerable<T>> GenerateObjects(int count);
}