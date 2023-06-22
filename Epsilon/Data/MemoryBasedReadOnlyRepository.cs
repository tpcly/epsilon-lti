using System.Linq.Expressions;
using Epsilon.Abstractions.Data;

namespace Epsilon.Data;

public class MemoryBasedReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class, IEntity
{
    public MemoryBasedReadOnlyRepository()
    {
        Store = new Dictionary<object, TEntity>();
    }

    public MemoryBasedReadOnlyRepository(IDictionary<object, TEntity> store)
    {
        Store = store;
    }

    public MemoryBasedReadOnlyRepository(IEnumerable<TEntity> entities)
    {
        Store = entities.ToDictionary(static e => e.Id!, static e => e);
    }

    protected IDictionary<object, TEntity> Store { get; }

    protected virtual IQueryable<TEntity?> GetQueryable(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string[]? includeProperties = null,
        int? skip = null,
        int? take = null
    )
    {
        var query = Store.Values.AsQueryable();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }

        if (take.HasValue)
        {
            query = query.Take(take.Value);
        }

        return query;
    }

    public IEnumerable<TEntity?> AllToList(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string[]? includeProperties = null,
        int? skip = null,
        int? take = null
    )
    {
        return GetQueryable(null, orderBy, includeProperties, skip, take).ToList();
    }

    public async Task<IEnumerable<TEntity?>> AllToListAsync(
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string[]? includeProperties = null,
        int? skip = null,
        int? take = null
    )
    {
        return AllToList(orderBy, includeProperties, skip, take);
    }

    public IEnumerable<TEntity?> ToList(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string[]? includeProperties = null,
        int? skip = null,
        int? take = null
    )
    {
        return GetQueryable(filter, orderBy, includeProperties, skip, take).ToList();
    }

    public async Task<IEnumerable<TEntity?>> ToListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string[]? includeProperties = null,
        int? skip = null,
        int? take = null
    )
    {
        return ToList(filter, orderBy, includeProperties, skip, take);
    }

    public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null)
    {
        return GetQueryable(filter, null, includeProperties).SingleOrDefault();
    }

    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string[]? includeProperties = null)
    {
        return SingleOrDefault(filter, includeProperties);
    }

    public TEntity? FirstOrDefault(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string[]? includeProperties = null
    )
    {
        return GetQueryable(filter, orderBy, includeProperties).FirstOrDefault();
    }

    public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string[]? includeProperties = null
    )
    {
        return FirstOrDefault(filter, orderBy, includeProperties);
    }

    public TEntity? Find(object? id)
    {
        Store.TryGetValue(id, out var entity);
        return entity;
    }

    public async Task<TEntity?> FindAsync(object? id)
    {
        return Find(id);
    }

    public int Count(Expression<Func<TEntity, bool>>? filter = null)
    {
        return GetQueryable(filter).Count();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return Count(filter);
    }

    public bool Any(Expression<Func<TEntity, bool>>? filter = null)
    {
        return GetQueryable(filter).Any();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return Any(filter);
    }
}