using Starly.Domain.Interfaces.Entities;
using Starly.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Starly.Infra.Data.Repositories;

public abstract class BaseRepository<TObject, G, TContext> : IBaseRepository<TObject, G>
   where TObject : class, IEntity<G>
   where TContext : DbContext
{
    protected TContext _dataContext;

    public BaseRepository(TContext context)
    {
        _dataContext = context;
    }

    public async Task InsertAsync(TObject obj)
    {
        await _dataContext.Set<TObject>().AddAsync(obj);
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TObject obj)
    {
        _dataContext.Entry(obj).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(G id)
    {
        _dataContext.Set<TObject>().Remove(await SelectAsync(id));
        await _dataContext.SaveChangesAsync();
    }

    public async Task<IList<TObject>> SelectAsync() =>
        await _dataContext.Set<TObject>().ToListAsync();

    public async Task<TObject> SelectAsync(G id) =>
        await _dataContext.Set<TObject>().FindAsync(id);

    public IQueryable<TObject> GetQueryable()
    {
        return _dataContext.Set<TObject>();
    }
}
