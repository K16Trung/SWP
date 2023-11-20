using Services.Entity;
using Services.Repository.Interface;
using Services.Service.Interface;
using Services.Service;
using Services.Commons;
using Microsoft.EntityFrameworkCore;

namespace Services.Repository
{
    public class PostRepo : GenericRepo<Post>, IPostRepo
    {
        private readonly AppDBContext _context;
        public PostRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
            _context = context;
        }

        public async Task<Pagination<Post>> GetAllPostDetail(int pageIndex = 1, int pageSize = 10)
        {
            var itemCount = await _context.Posts.CountAsync();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            var post = await _context.Posts.Skip((pageIndex - 1) * pageSize)
                                                 .Take(pageSize)
                                                 .AsNoTracking()
                                                 .ToListAsync();

            var result = new Pagination<Post>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = itemCount,
                Items = post
            };

            return result;
        }

        public async Task<Pagination<Post>> GetPostDetailById(int id, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var totalCourseCount = await _dbSet.CountAsync(x => x.Id == id);

            var post = await _dbSet
                .Where(x => x.Id == id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<Post>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = totalCourseCount,
                Items = post
            };

            return result;
        }
    }
}
