using Services.Entity;
using Services.Repository.Interface;
using Services.Service;
using Services.Service.Interface;

namespace Services.Repository
{
    public class FeedbackRepo : GenericRepo<Feedback>, IFeedbackRepo
    {
        public FeedbackRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
        }
    }
}
