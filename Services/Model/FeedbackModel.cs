using Services.Entity;

namespace Services.Model
{
    public class FeedbackModel
    {
        public string Content { get; set; }
        public int CourseEnrollCourseId { get; set; }
        public int CourseEnrollUserId { get; set; }
    }

    public class FeedbackResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int CourseEnrollCourseId { get; set; }
        public string? CourseName { get; set; }
        public int CourseEnrollUserId { get; set; }
        public string? UserName { get; set; }
    }
}
