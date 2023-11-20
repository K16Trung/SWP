using Services.Commons;
using Services.Model;

namespace Services.Service
{
    public interface ICourseServices
    {
        Task<ResponseModel<CourseResponse>> CreateCourse(CourseModel course);
        Task<ResponseModel<Pagination<CourseModel>>> GetAllCourse(int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<Pagination<CourseModel>>> GetCourseById(int id, int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<Pagination<CourseModel>>> GetCourseByName(string coursename, int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<string>> UpdateCourse(int id, CourseModel courseModel);
        Task<ResponseModel<string>> RemoveCourse(int id);
    }
}
