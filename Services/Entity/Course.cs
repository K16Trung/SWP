using Services.Enum;

namespace Services.Entity
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public decimal Price { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<Material>? Materials { get; set; }
        public ICollection<CourseEnroll>? CourseEnrolls { get; set; }

    }
}
