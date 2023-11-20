

using Services.Enum;

namespace Services.Entity
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}
