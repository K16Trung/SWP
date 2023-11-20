using Services.Entity;
using Services.Repository.Interface;
using Services.Service;
using Services.Service.Interface;

namespace Services.Repository
{
    public class CourseEnrollRepo : GenericRepo<CourseEnroll>, ICourseEnrollRepo
    {
        public CourseEnrollRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
        }
    }
}
