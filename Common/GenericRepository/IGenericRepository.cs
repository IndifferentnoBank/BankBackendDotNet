using System.Linq.Expressions;

namespace Common.GenericRepository;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>?> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

}