using System.Linq.Expressions;

namespace Common.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> Query();
    IQueryable<TEntity> QueryTracking();

    ValueTask<TEntity?> GetByIdAsync(
        object id,
        CancellationToken cancellationToken = default);

    void Add(TEntity entity);

    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);

    void DeleteRange(IEnumerable<TEntity> entities);

    Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);
}