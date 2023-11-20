using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using Services.Commons;
using Services.Entity;
using Services.Enum;
using Services.Service;
using Services.Service.Interface;
using System;
using System.Linq.Expressions;

namespace Services.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        protected readonly DbSet<T> _dbSet;
        private readonly ICurrentTimeService _currentTime;
        private readonly IClaimsServices _claimsServices;

        public GenericRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices)
        {
            _dbSet = context.Set<T>();
            _currentTime = currentTime;
            _claimsServices = claimsServices;
        }

        public async Task CreateAsync(T entity)
        {
            entity.CreationDate = _currentTime.GetCurrentTime();
            var id = _claimsServices.GetCurrentUser();
            entity.CreatedBy = !string.IsNullOrEmpty(id) ? int.Parse(id) : null;
            await _dbSet.AddAsync(entity);
        }

        public async Task CreateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreationDate = _currentTime.GetCurrentTime();
                var id = _claimsServices.GetCurrentUser();
                entity.CreatedBy = !string.IsNullOrEmpty(id) ? int.Parse(id) : null;
            }
            await _dbSet.AddRangeAsync(entities);
        }
        public void UpdateAsync(T entity)
        {
            entity.ModificationDate = _currentTime.GetCurrentTime();
            var id = _claimsServices.GetCurrentUser();
            entity.ModificationBy = int.Parse(id);
            _dbSet.Update(entity);
        }

        public void UpdateRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                entity.ModificationDate = _currentTime.GetCurrentTime();
                var id = _claimsServices.GetCurrentUser();
                entity.ModificationBy = int.Parse(id);
            }
            _dbSet.UpdateRange(entities);
        }
        public void DeleteAsync(T entity)
        {
            entity.DeletionDate = _currentTime.GetCurrentTime();
            var id = _claimsServices.GetCurrentUser();
            entity.DeleteBy = int.Parse(id);
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }

        public async Task<Pagination<T>> FilterEntity(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IQueryable<T>> include = null,
            int pageIndex = 0, int pageSize = 10,
            Expression<Func<T, object>> sortColumn = null,
            SortDirection sortDirection = SortDirection.Descending)
        {
            var query = _dbSet.AsQueryable();

            // Include related entities if specified
            if (include is not null)
            {
                query = include(query);
            }

            // Apply filtering if specified
            if (expression is not null)
            {
                query = query.Where(expression);
            }

            // Apply sorting if specified
            if (sortColumn is not null)
            {
                query = sortDirection == SortDirection.Ascending
                    ? query.OrderBy(sortColumn)
                    : query.OrderByDescending(sortColumn);
            }

            // Calculate the total item count for the query
            var itemCount = await query.CountAsync();

            // Apply pagination to the query
            var items = await query.Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            // Create the pagination result
            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = itemCount,
                Items = items,
            };

            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();


        public async Task<T> GetEntityByIdAsync(int id) => await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<T> GetEntityCondition(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include is not null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(expression);
        }

        public async Task<Pagination<T>> ToPagination(int pageIndex = 0, int pageSize = 10)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.OrderByDescending(x => x.CreationDate)
                                    .Skip(pageIndex * pageSize)
                                    .Take(pageSize)
                                    .AsNoTracking()
                                    .ToListAsync();

            var result = new Pagination<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = itemCount,
                Items = items,
            };
            return result;
        }
    }
}
