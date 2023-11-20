using Services.Entity;
using Services.Enum;

namespace Services.Model
{
    public class OrderModel
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int UserId { get; set; }
        public IEnumerable<OrderDetailModel> Details { get; set; } = new List<OrderDetailModel>();
    }

    public class OrderDetailModel
    {
        public int CourseId { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public decimal CoursePrice { get; set; }
    }

    public class OrderResponse
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

        public IEnumerable<OrderDetailViewModel> Details { get; set; } = new List<OrderDetailViewModel>();
    }
}
