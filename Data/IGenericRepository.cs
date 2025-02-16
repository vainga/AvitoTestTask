using System.Linq.Expressions;

namespace AvitoTestTask.Data
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> UpdateAsync(T entity);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
