using AutoMapper;
using Services.Commons;
using Services.Entity;
using Services.Model;

namespace Services.Service
{
    public class CourseServices : ICourseServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsServices _claimsServices;
        public CourseServices(IUnitOfWork unitOfWork, IMapper mapper, IClaimsServices claimsServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsServices = claimsServices;
        }

        public async Task<ResponseModel<CourseResponse>> CreateCourse(CourseModel course)
        {

            var user = _claimsServices.GetCurrentUser();
            if (user is null)
            {
                return new ResponseModel<CourseResponse>
                {
                    Errors = "User not logged in."
                };
            }
            var courseObj = _mapper.Map<Course>(course);
            courseObj.UserId = int.Parse(user);
            courseObj.Status = Enum.Status.Disable;
            await _unitOfWork.courseRepo.CreateAsync(courseObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync();
            if (isSuccess > 0)
            {
                var result = _mapper.Map<CourseResponse>(courseObj);
               
                return new ResponseModel<CourseResponse>
                {
                    Data = result
                };
            }
            return new ResponseModel<CourseResponse> { Errors = "Create Course failed." };
        }
    

        public async Task<ResponseModel<Pagination<CourseModel>>> GetAllCourse(int pageIndex = 1, int pageSize = 10)
        {
            var courseObj = await _unitOfWork.courseRepo.GetAllCourseDetail(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<CourseModel>>(courseObj);
            if (courseObj.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<CourseModel>> { Errors = "Not found" };
            }
            return new ResponseModel<Pagination<CourseModel>> { Data = result };
        }

        public async Task<ResponseModel<Pagination<CourseModel>>> GetCourseById(int id, int pageIndex = 1, int pageSize = 10)
        {
            var courseObj = await _unitOfWork.courseRepo.GetCourseDetailById(id, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<CourseModel>>(courseObj);
            if (courseObj.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<CourseModel>> { Errors = "Not found" };
            }
            return new ResponseModel<Pagination<CourseModel>> { Data = result };

        }

        public async Task<ResponseModel<Pagination<CourseModel>>> GetCourseByName(string coursename, int pageIndex = 1, int pageSize = 10)
        {
            var courseObj = await _unitOfWork.courseRepo.GetCourseDetailByName(coursename, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<CourseModel>>(courseObj);

            if (courseObj.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<CourseModel>> { Errors = "Not found" };
            }

            return new ResponseModel<Pagination<CourseModel>> { Data = result };
        }

        

        public async Task<ResponseModel<string>> RemoveCourse(int id)
        {
            var courseObj = await _unitOfWork.courseRepo.GetEntityByIdAsync(id);
            if (courseObj is not null)
            {
                _unitOfWork.courseRepo.DeleteAsync(courseObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    return new ResponseModel<string> { Data = "Success" };
                }
            }
            return new ResponseModel<string> { Errors = "Fail!" };
        }

        public async Task<ResponseModel<string>> UpdateCourse(int id, CourseModel courseModel)
        {
            var courseObj = await _unitOfWork.courseRepo.GetEntityByIdAsync(id);
            if (courseObj is not null)
            {
                _mapper.Map(courseModel, courseObj);
                _unitOfWork.courseRepo.UpdateAsync(courseObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    return new ResponseModel<string> { Data = "Success" };
                }
            }
            return new ResponseModel<string> { Errors = "Fail!" };
        }
    }
}
