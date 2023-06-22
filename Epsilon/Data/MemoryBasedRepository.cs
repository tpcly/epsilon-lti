using Epsilon.Abstractions.Data;

namespace Epsilon.Data;

public class MemoryBasedRepository<TEntity> : MemoryBasedReadOnlyRepository<TEntity>, IRepository<TEntity>
    where TEntity : class, IEntity
{
    private readonly IDictionary<object, TEntity> _updateTracker = new Dictionary<object, TEntity>();
    private readonly IList<object> _deleteTracker = new List<object>();

    public MemoryBasedRepository(IDictionary<object, TEntity>? store = default) : base(store)
    {
    }

    public TEntity Create(TEntity entity)
    {
        _updateTracker[entity.Id] = entity;
        return entity;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        return Create(entity);
    }

    public void Update(TEntity entity)
    {
        _updateTracker[entity.Id] = entity;
    }

    public void Delete(object id)
    {
        _deleteTracker.Add(id);
    }

    public async Task DeleteAsync(object id)
    {
        Delete(id);
    }

    public void Delete(TEntity entity)
    {
        Delete(entity.Id);
    }

    public int SaveChanges()
    {
        foreach (var (key, value) in _updateTracker)
        {
            Store[key] = value;
        }

        foreach (var key in _deleteTracker)
        {
            Store.Remove(key);
        }

        return _updateTracker.Count + _deleteTracker.Count;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return SaveChanges();
    }
}