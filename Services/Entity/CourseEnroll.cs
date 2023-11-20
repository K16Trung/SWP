
namespace Services.Entity
{
    public class CourseEnroll : BaseEntity
    {
        public DateTime EnrollmentDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
    }
}
