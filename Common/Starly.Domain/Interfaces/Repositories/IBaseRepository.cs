namespace Starly.Domain.Interfaces.Repositories;

public interface IBaseRepository<T, G> where T : class
{
    Task InsertAsync(T obj);
    Task UpdateAsync(T obj);
    Task DeleteAsync(G id);
    Task<IList<T>> SelectAsync();
    Task<T> SelectAsync(G id);
}