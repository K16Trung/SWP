
namespace Services.Entity
{
    public class OrderDetail : BaseEntity
    {
        public Order Order { get; set; }
        public int OrderId { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
    }
}
