using AutoMapper;
using Services.Commons;
using Services.Entity;
using Services.Model;
using Services.Service.Interface;

namespace Services.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTSection _jwtSection;
        private readonly IMapper _mapper;
        private readonly IClaimsServices _claimsServices;
        public FeedbackService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimsServices claimsServices,
            JWTSection jwtSection)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsServices = claimsServices;
            _jwtSection = jwtSection;
        }
        public async Task<ResponseModel<FeedbackResponse>> CreateFeedback(FeedbackModel model)
        {
            var user = _claimsServices.GetCurrentUser();
            if (user is null)
            {
                return new ResponseModel<FeedbackResponse>
                {
                    Errors = "User not logged in."
                };
            }

            var courseEnroll = await _unitOfWork.courseEnrollRepo.GetEntityCondition(c => c.CourseId == model.CourseEnrollCourseId && c.UserId == model.CourseEnrollUserId);
            if (courseEnroll is null)
            {
                return new ResponseModel<FeedbackResponse>
                {
                    Errors = "Course enroll not found."
                };
            }

            //if (courseEnroll.UserId != int.Parse(user))
            //{
            //    return new ResponseModel<FeedbackResponse>
            //    {
            //        Errors = "Course enroll not found."
            //    };
            //}

            var feedback = _mapper.Map<Feedback>(model);
            feedback.FeedbackDate = DateTime.UtcNow;
            await _unitOfWork.feedbackRepo.CreateAsync(feedback);
            var isSuccess = await _unitOfWork.SaveChangeAsync();
            if (isSuccess > 0)
            {
                var result = _mapper.Map<FeedbackResponse>(feedback);

                return new ResponseModel<FeedbackResponse>
                {
                    Data = result
                };
            }
            return new ResponseModel<FeedbackResponse> { Errors = "Create Feedback failed." };
        }

        public async Task<ResponseModel<string>> DeleteFeedback(int id)
        {
            var feedback = await _unitOfWork.feedbackRepo.GetEntityByIdAsync(id);
            if (feedback is not null)
            {
                _unitOfWork.feedbackRepo.DeleteAsync(feedback);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    return new ResponseModel<string> { Data = "Success" };
                }
            }
            return new ResponseModel<string> { Errors = "Fail!" };
        }

        public async Task<ResponseModel<FeedbackResponse>> GetFeedbackById(int id)
        {
            var feedback = await _unitOfWork.feedbackRepo.GetEntityByIdAsync(id);
            if (feedback is null)
            {
                return new ResponseModel<FeedbackResponse> { Errors = "Not found" };
            }

            var result = _mapper.Map<FeedbackResponse>(feedback);
            var user = await _unitOfWork.userRepo.GetEntityByIdAsync(result.CourseEnrollUserId);
            var course = await _unitOfWork.courseRepo.GetEntityByIdAsync(result.CourseEnrollCourseId);
            result.UserName = user?.Email ?? string.Empty;
            result.CourseName = course?.CourseName ?? string.Empty;

            return new ResponseModel<FeedbackResponse> { Data = result };
        }

        public async Task<ResponseModel<Pagination<FeedbackResponse>>> GetFeedbacks(int pageIndex = 1, int pageSize = 10)
        {
            var feedbacks = await _unitOfWork.feedbackRepo.ToPagination(pageIndex - 1, pageSize);
            if (feedbacks.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<FeedbackResponse>> { Errors = "Not found" };
            }

            var result = _mapper.Map<Pagination<FeedbackResponse>>(feedbacks);
            foreach (var item in result.Items)
            {
                var user = await _unitOfWork.userRepo.GetEntityByIdAsync(item.CourseEnrollUserId);
                var course = await _unitOfWork.courseRepo.GetEntityByIdAsync(item.CourseEnrollCourseId);
                item.UserName = user?.Email ?? string.Empty;
                item.CourseName = course?.CourseName ?? string.Empty;

            }
            return new ResponseModel<Pagination<FeedbackResponse>> { Data = result };
        }

        public Task<ResponseModel<Pagination<FeedbackResponse>>> GetFeedbacksByCourse(int courseId, int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<FeedbackResponse>> UpdateFeedback(int id, FeedbackModel model)
        {
            var feedback = await _unitOfWork.feedbackRepo.GetEntityByIdAsync(id);
            if (feedback is not null)
            {
                _mapper.Map(model, feedback);
                _unitOfWork.feedbackRepo.UpdateAsync(feedback);
                var isSuccess = await _unitOfWork.SaveChangeAsync();

                if (isSuccess > 0)
                {
                    var result = _mapper.Map<FeedbackResponse>(feedback);

                    return new ResponseModel<FeedbackResponse> { Data = result };
                }
            }

            return new ResponseModel<FeedbackResponse> { Errors = "Fail!" };
        }
    }
}
