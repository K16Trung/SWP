using Microsoft.EntityFrameworkCore;
using Services.Commons;
using Services.Entity;
using Services.Repository.Interface;
using Services.Service;
using Services.Service.Interface;
using System;

namespace Services.Repository
{
    public class CourseRepo : GenericRepo<Course>, ICourseRepo
    {
        private readonly AppDBContext _context;
        public CourseRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
            _context = context;
        }

        public async Task<Pagination<Course>> GetAllCourseDetail(int pageIndex = 1, int pageSize = 10)
        {
            var itemCount = await _context.Courses.CountAsync();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            var course = await _context.Courses.Skip((pageIndex - 1) * pageSize)
                                                 .Take(pageSize)
                                                 .AsNoTracking()
                                                 .ToListAsync();

            var result = new Pagination<Course>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = itemCount,
                Items = course
            };

            return result;
        }

        public async Task<Pagination<Course>> GetCourseDetailById(int id, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var totalCourseCount = await _dbSet.CountAsync(x => x.Id == id);

            var course = await _dbSet
                .Where(x => x.Id == id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<Course>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = totalCourseCount,
                Items = course
            };

            return result;
        }

        public async Task<Pagination<Course>> GetCourseDetailByName(string coursename, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Courses
                .Where(course => course.CourseName.Contains(coursename));

            var itemCount = await query.CountAsync();

            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var courses = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            var result = new Pagination<Course>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = itemCount,
                Items = courses
            };
            return result;
        }
    }
}
