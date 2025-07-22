using System.Linq.Expressions;

namespace PRN232_Assignment.UserService.Repository.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> AddAsync(T obj);
        Task<bool> DeleteAsync(object obj);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(object id);
        Task<T> GetSingleByConditionAsynce(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes);
        Task<int> UpdateAsync(T obj);
    }
}