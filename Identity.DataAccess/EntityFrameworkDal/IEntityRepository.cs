
using System.Linq.Expressions;

namespace Identity.DataAccess.EntityFrameworkDal
{
    public interface IEntityRepository<T> where T : class, new()
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);

    }
}