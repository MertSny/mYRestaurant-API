using Core.Entites;
using Core.Helpers;
using System.Linq.Expressions;

namespace Core.DataAccess.Abstract
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity, bool withCollections = true);
        Task<IEnumerable<T>> Search(bool eager = false, IEnumerable<string> includes = null);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate, bool eager = false, IEnumerable<string> includes = null);
        Task<IEnumerable<T>> Search(Expression<Func<T, bool>> predicate, bool eager = false, IEnumerable<string> includes = null, Expression<Func<T, object>> orderSelector = null, bool orderAsc = false);
        Task<PagedResult<T>> Search(int page, int pageSize, Expression<Func<T, bool>> predicate = null, Expression<Func<T, object>> orderSelector = null, bool orderAsc = false, bool eager = false, IEnumerable<string> includes = null);
        Task<int> UpdateFieldsSave(T entity, params Expression<Func<T, object>>[] includeProperties);
        Task<int> Count(Expression<Func<T, bool>> predicate = null);
    }
}
