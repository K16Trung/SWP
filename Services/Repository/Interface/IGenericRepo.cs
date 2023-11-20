using Microsoft.EntityFrameworkCore.Query;
using Services.Commons;
using Services.Entity;
using Services.Enum;
using System.Linq.Expressions;

namespace Services.Repository
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        Task CreateAsync(T entity);
        Task CreateRangeAsync(IEnumerable<T> entities);
        void UpdateAsync(T entity);
        void UpdateRangeAsync(IEnumerable<T> entities);
        void DeleteAsync(T entity);
        Task<T> GetEntityByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetEntityCondition(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<Pagination<T>> ToPagination(int pageIndex = 0, int pageSize = 10);
        Task<Pagination<T>> FilterEntity(Expression<Func<T, bool>> expression = null,
        Func<IQueryable<T>, IQueryable<T>> include = null,
        int pageIndex = 0,
        int pageSize = 10,
        Expression<Func<T, object>> sortColumn = null,
        SortDirection sortDirection = SortDirection.Descending);
    }
}
