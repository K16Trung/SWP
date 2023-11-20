using AutoMapper;
using Services.Commons;
using Services.Entity;
using Services.Model;
using Services.Service.Interface;
using System.Text.RegularExpressions;

namespace Services.Service
{
    public class OrderServices : IOrderServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTSection _jwtSection;
        private readonly IMapper _mapper;
        private readonly IClaimsServices _claimsServices;
        public OrderServices(IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimsServices claimsServices,
            JWTSection jwtSection)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsServices = claimsServices;
            _jwtSection = jwtSection;
        }

        public async Task<ResponseModel<OrderResponse>> CreateOrder(OrderModel orderModel)
        {
            var user = _claimsServices.GetCurrentUser();
            if (user is null)
            {
                return new ResponseModel<OrderResponse>
                {
                    Errors = "User not logged in."
                };
            }

            var order = _mapper.Map<Order>(orderModel);
            order.UserId = int.Parse(user);
            order.OrderStatus = Enum.OrderStatus.Pending;
            var transaction = await _unitOfWork.BeginTransaction();
            await _unitOfWork.orderRepo.CreateAsync(order);
            var isSuccess = await _unitOfWork.SaveChangeAsync();

            if (isSuccess > 0)
            {
                foreach (var item in orderModel.Details)
                {
                    var course = await _unitOfWork.courseRepo.GetEntityByIdAsync(item.CourseId);
                    if (course is null)
                    {
                        await transaction.RollbackAsync();
                        return new ResponseModel<OrderResponse> { Errors = "CourseNot Found." };

                    }
                    var orderDetail = _mapper.Map<OrderDetail>(item);
                    orderDetail.OrderId = order?.Id ?? 0;

                    await _unitOfWork.orderDetailRepo.CreateAsync(orderDetail);
                }

                isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    await transaction.CommitAsync();

                    var result = _mapper.Map<OrderResponse>(order);

                    return new ResponseModel<OrderResponse>
                    {
                        Data = result
                    };
                }
                await transaction.RollbackAsync();
            }

            return new ResponseModel<OrderResponse> { Errors = "Create Order failed." };
        }

        public async Task<ResponseModel<OrderResponse>> GetOrderById(int id)
        {
            var order = await _unitOfWork.orderRepo.GetEntityByIdAsync(id);
            if (order is null)
            {
                return new ResponseModel<OrderResponse> { Errors = "Not found" };
            }


            var response = _mapper.Map<OrderResponse>(order);
            response.UserName = (await _unitOfWork.userRepo.GetEntityByIdAsync(response.UserId))?.Email ?? "";
            var orderDetail = await _unitOfWork.orderDetailRepo.FilterEntity(detail => detail.OrderId == id, pageSize: int.MaxValue);
            var orderDetailViewList = _mapper.Map<Pagination<OrderDetailViewModel>>(orderDetail);

            foreach (var item in orderDetailViewList.Items)
            {
                var course = await _unitOfWork.courseRepo.GetEntityByIdAsync(item.CourseId);
                item.CourseName = course?.CourseName ?? string.Empty;
                item.CoursePrice = course?.Price ?? 0;
            }

            response.Details = orderDetailViewList.Items;

            return new ResponseModel<OrderResponse> { Data = response };

        }

        public async Task<ResponseModel<Pagination<OrderResponse>>> GetOrders(int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            var orderObj = await _unitOfWork.orderRepo.ToPagination(pageIndex - 1, pageSize);
            if (orderObj.Items.Count < 1)
            {
                return new ResponseModel<Pagination<OrderResponse>> { Errors = "Not found" };
            }

            var result = _mapper.Map<Pagination<OrderResponse>>(orderObj);

            return new ResponseModel<Pagination<OrderResponse>> { Data = result };
        }

        public async Task<ResponseModel<OrderResponse>> UpdateOrder(int id, OrderModel orderModel)
        {
            var user = _claimsServices.GetCurrentUser();
            if (user is null)
            {
                return new ResponseModel<OrderResponse>
                {
                    Errors = "User not logged in."
                };
            }
            var order = await _unitOfWork.orderRepo.GetEntityByIdAsync(id);

            if (order is null)
            {
                return new ResponseModel<OrderResponse> { Errors = "Not found" };
            }

            _mapper.Map(orderModel, order);
            order.UserId = int.Parse(user);

            var transaction = await _unitOfWork.BeginTransaction();
            _unitOfWork.orderRepo.UpdateAsync(order);
            var isSuccess = await _unitOfWork.SaveChangeAsync();

            if (isSuccess > 0)
            {
                if (orderModel.Details.Any())
                {
                    await _unitOfWork.orderDetailRepo.DeleteDetails(orderModel.Details.Select(d => d.CourseId));
                    isSuccess = await _unitOfWork.SaveChangeAsync();

                    foreach (var item in orderModel.Details)
                    {
                        var orderDetail = _mapper.Map<OrderDetail>(item);
                        orderDetail.OrderId = order?.Id ?? 0;

                        await _unitOfWork.orderDetailRepo.CreateAsync(orderDetail);
                    }

                    isSuccess = await _unitOfWork.SaveChangeAsync();
                    if (isSuccess > 0)
                    {
                        await transaction.CommitAsync();

                        var result = _mapper.Map<OrderResponse>(order);

                        return new ResponseModel<OrderResponse>
                        {
                            Data = result
                        };
                    }
                }
            }

            await transaction.RollbackAsync();
            return new ResponseModel<OrderResponse> { Errors = "Update Order failed." };
        }

        /// <summary>
        /// Payment for an existed order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<ResponseModel<OrderResponse>> Payment(int orderId, PaymentModel paymentModel)
        {
            paymentModel.CourseIds = default;
            var order = await _unitOfWork.orderRepo.GetEntityByIdAsync(orderId);
            if (order is null)
            {
                return new ResponseModel<OrderResponse> { Errors = "Not found" };
            }

            if (order.OrderStatus == Services.Enum.OrderStatus.Success)
            {
                return new ResponseModel<OrderResponse> { Errors = "Order was already paid." };
            }


            if (string.IsNullOrWhiteSpace(paymentModel.CVV)
                || string.IsNullOrWhiteSpace(paymentModel.CardNumber)
                || string.IsNullOrWhiteSpace(paymentModel.CardName)
                || string.IsNullOrWhiteSpace(paymentModel.CardDueDate))
            {
                return new ResponseModel<OrderResponse> { Errors = "Card infomation is incorect." };
            }

            order.OrderStatus = Enum.OrderStatus.Success;
            var payment = _mapper.Map<Payment>(paymentModel);
            payment.OrderId = order.Id;
            var transaction = await _unitOfWork.BeginTransaction();
            await _unitOfWork.paymentRepo.CreateAsync(payment);
            _unitOfWork.orderRepo.UpdateAsync(order);
            var result = await _unitOfWork.SaveChangeAsync();

            if (result > 0)
            {
                await transaction.CommitAsync();
                return new ResponseModel<OrderResponse> { Data = _mapper.Map<OrderResponse>(order) };
            }

            await transaction.RollbackAsync();
            order.OrderStatus = Enum.OrderStatus.Fail;
            return new ResponseModel<OrderResponse> { Errors = "Payment failed." };
        }

        /// <summary>
        /// Payment for courses that not in an order.
        /// </summary>
        /// <param name="paymentModel"></param>
        /// <returns></returns>
        public async Task<ResponseModel<OrderResponse>> Payment(PaymentModel paymentModel)
        {
            var user = _claimsServices.GetCurrentUser();
            if (user is null)
            {
                return new ResponseModel<OrderResponse>
                {
                    Errors = "User not logged in."
                };
            }

            if (paymentModel.CourseIds?.Any() != true)
            {
                return new ResponseModel<OrderResponse> { Errors = "Courses are empty." };
            }

            if (string.IsNullOrWhiteSpace(paymentModel.CVV)
                || string.IsNullOrWhiteSpace(paymentModel.CardNumber)
                || string.IsNullOrWhiteSpace(paymentModel.CardName)
                || string.IsNullOrWhiteSpace(paymentModel.CardDueDate))
            {
                return new ResponseModel<OrderResponse> { Errors = "Card infomation is incorect." };
            }

            var courseModels = new List<CourseModel>();
            var orderDetailModels = new List<OrderDetailModel>();
            foreach (var item in paymentModel.CourseIds)
            {
                var course = await _unitOfWork.courseRepo.GetEntityByIdAsync(item);
                if (course is null)
                {
                    return new ResponseModel<OrderResponse> { Errors = "Course(s) are not found." };
                }

                var courseModel = _mapper.Map<CourseModel>(course);
                courseModels.Add(courseModel);
                orderDetailModels.Add(new() { CourseId = item });
            }

            OrderModel orderModel = new()
            {
                OrderDate = DateTime.UtcNow,
                UserId = int.Parse(user),
                TotalPrice = courseModels.Select(c => c.Price).Sum(),
                Details = orderDetailModels
            };

            var createResult = await CreateOrder(orderModel);

            if (createResult.HasError)
            {
                return createResult;
            }

            var payment = _mapper.Map<Payment>(paymentModel);
            payment.OrderId = createResult.Data.Id;
            await _unitOfWork.paymentRepo.CreateAsync(payment);

            var order = _mapper.Map<Order>(orderModel);
            _unitOfWork.orderRepo.UpdateAsync(order);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                return createResult;
            }

            return new ResponseModel<OrderResponse> { Errors = "Payment failed." };

        }

        public async Task<ResponseModel<Payment>> GetPaymentById(int id)
        {
            var payment = await _unitOfWork.paymentRepo.GetEntityByIdAsync(id);
            if (payment is null)
            {
                return new ResponseModel<Payment> { Errors = "Not found" };
            }

            return new ResponseModel<Payment> { Data = payment };

        }

        public async Task<ResponseModel<Pagination<Payment>>> GetPayments(int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1) pageIndex = 1;
            var orderObj = await _unitOfWork.paymentRepo.ToPagination(pageIndex - 1, pageSize);
            if (orderObj.Items.Count < 1)
            {
                return new ResponseModel<Pagination<Payment>> { Errors = "Not found" };
            }

            var result = _mapper.Map<Pagination<Payment>>(orderObj);

            return new ResponseModel<Pagination<Payment>> { Data = result };
        }
    }
}
