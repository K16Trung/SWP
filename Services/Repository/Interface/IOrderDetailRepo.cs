using Services.Entity;

namespace Services.Repository.Interface
{
    public interface IOrderDetailRepo : IGenericRepo<OrderDetail>
    {
        Task DeleteDetails(IEnumerable<int> detailId);
    }
}
