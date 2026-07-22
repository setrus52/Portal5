using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Common.Repositories;

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private DbContext Context { get; }

    private DbSet<TEntity> Set { get; }

    public Repository(DbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        Context = context;
        Set = context.Set<TEntity>();
    }

    public virtual IQueryable<TEntity> Query()
    {
        return Set.AsNoTracking();
    }

    public virtual ValueTask<TEntity?> GetByIdAsync(
        object id,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return Set.FindAsync([id], cancellationToken);
    }

    public virtual void Add(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        Set.Add(entity);
    }

    public virtual void AddRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        Set.AddRange(entities);
    }

    public virtual void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var entry = Context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            Set.Attach(entity);
        }

        entry.State = EntityState.Modified;
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            Update(entity);
        }
    }

    public virtual void Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var entry = Context.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            Set.Attach(entity);
        }

        Set.Remove(entity);
    }

    public virtual void DeleteRange(IEnumerable<TEntity> entities)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            Delete(entity);
        }
    }

    public virtual Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return Set.AnyAsync(predicate, cancellationToken);
    }

    public virtual Task<int> CountAsync(
        CancellationToken cancellationToken = default)
    {
        return Set.CountAsync(cancellationToken);
    }

    public virtual Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return Set.CountAsync(predicate, cancellationToken);
    }
}