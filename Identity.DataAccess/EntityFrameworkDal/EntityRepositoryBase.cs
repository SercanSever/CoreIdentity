
using System.Linq;
using System.Linq.Expressions;
using Identity.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.DataAccess.EntityFrameworkDal
{
    public class EntityRepositoryBase<T> : IEntityRepository<T> where T : class, new()
    {
        private readonly IdentityContext _context;
        public EntityRepositoryBase()
        {
            _context = new IdentityContext();
        }
        public async Task<bool> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(T entity)
        {
            var deletedEntity = _context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter = null)
        {
            return filter == null
                ? await _context.Set<T>().ToListAsync()
                : await _context.Set<T>().Where(filter).ToListAsync();
        }
        public async Task<T> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> filter)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(filter);
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var updatedEntity = _context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}