using Services.Enum;

namespace Services.Entity
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public Role Role { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<CourseEnroll>? CourseEnrolls { get; set; }
    }
}
