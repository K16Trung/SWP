using Services.Commons;
using Services.Entity;

namespace Services.Repository.Interface
{
    public interface ICourseRepo : IGenericRepo<Course>
    {
        Task<Pagination<Course>> GetAllCourseDetail(int pageIndex = 1, int pageSize = 10);
        Task<Pagination<Course>> GetCourseDetailById(int id, int pageIndex = 1, int pageSize = 10);
        Task<Pagination<Course>> GetCourseDetailByName(string coursename, int pageIndex = 1, int pageSize = 10);
    }
}
