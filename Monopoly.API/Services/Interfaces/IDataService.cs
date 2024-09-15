namespace Monopoly.API.Services.Interfaces;

/// <summary>
///     Defines an interface for serivices implements data retrieving logic
/// </summary>
/// <typeparam name="T">One of entities</typeparam>
public interface IDataService<T> where T : class
{
    /// <summary>
    ///     Retrieves an T object asynchronously
    /// </summary>
    /// <returns></returns>
    Task<T?> GetByIdAsync(long id);

    /// <summary>
    ///     Retrieves collection typed as T asynchronously from database or from cache
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>?> GetAllAsync();

    /// <summary>
    ///     Deletes an entity with specified id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool?> DeleteByIdAsync(long id);

    /// <summary>
    ///     Creates new T
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T?> Create(T entity);
}