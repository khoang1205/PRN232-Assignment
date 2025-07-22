using Microsoft.EntityFrameworkCore;
using PRN232_Assignment.UserService.Repository.Data;
using System.Linq.Expressions;

namespace PRN232_Assignment.UserService.Repository.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected UserDbContext _context = null;
        private DbSet<T> _table = null;

        public GenericRepository(UserDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _table.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> GetSingleByConditionAsynce(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<int> AddAsync(T obj)
        {
            await _table.AddAsync(obj);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T obj)
        {
            _table.Update(obj);
            return await _context.SaveChangesAsync();

        }

        public async Task<bool> DeleteAsync(object obj)
        {
            _table.Remove((T)obj);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
