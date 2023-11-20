using Services.Entity;
using Services.Repository.Interface;
using Services.Service;
using Services.Service.Interface;

namespace Services.Repository
{
    public class OrderDetailRepo : GenericRepo<OrderDetail>, IOrderDetailRepo
    {

        public OrderDetailRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
        }

        public async Task DeleteDetails(IEnumerable<int> detailId)
        {
            foreach (int id in detailId)
            {
                var orderDetail = await GetEntityByIdAsync(id);
                if(orderDetail != null)
                {
                    orderDetail.IsDeleted = true;
                }
            }
        }
    }
}
