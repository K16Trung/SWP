using Services.Entity;
using Services.Repository.Interface;
using Services.Service;
using Services.Service.Interface;

namespace Services.Repository
{
    public class PaymentRepo : GenericRepo<Payment>, IPaymentRepo
    {
        public PaymentRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
        }
    }
}
