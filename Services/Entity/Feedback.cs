
namespace Services.Entity
{
    public class Feedback : BaseEntity
    {
        public string Content { get; set; }
        public DateTime FeedbackDate { get; set; }
        public CourseEnroll CourseEnroll { get; set; }
        public int CourseEnrollId { get; set; } 
    }
}
