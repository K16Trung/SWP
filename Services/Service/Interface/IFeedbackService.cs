using Services.Commons;
using Services.Model;

namespace Services.Service.Interface
{
    public interface IFeedbackService
    {
        Task<ResponseModel<FeedbackResponse>> CreateFeedback(FeedbackModel model);
        Task<ResponseModel<Pagination<FeedbackResponse>>> GetFeedbacks(int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<Pagination<FeedbackResponse>>> GetFeedbacksByCourse(int courseId, int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<FeedbackResponse>> GetFeedbackById(int id);
        Task<ResponseModel<FeedbackResponse>> UpdateFeedback(int id, FeedbackModel model);
        Task<ResponseModel<string>> DeleteFeedback(int id);
    }
}
