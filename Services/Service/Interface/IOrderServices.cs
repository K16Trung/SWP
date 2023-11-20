using Services.Commons;
using Services.Entity;
using Services.Model;

namespace Services.Service.Interface
{
    public interface IOrderServices
    {
        Task<ResponseModel<OrderResponse>> CreateOrder(OrderModel orderModel);
        Task<ResponseModel<Pagination<OrderResponse>>> GetOrders(int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<OrderResponse>> GetOrderById(int id);
        Task<ResponseModel<OrderResponse>> UpdateOrder(int id, OrderModel orderModel);
        Task<ResponseModel<OrderResponse>> Payment(int orderId, PaymentModel paymentModel);
        Task<ResponseModel<OrderResponse>> Payment(PaymentModel paymentModel);
        Task<ResponseModel<Payment>> GetPaymentById(int id);
        Task<ResponseModel<Pagination<Payment>>> GetPayments(int pageIndex = 1, int pageSize = 10);
    }
}
