using Services.Entity;
using Services.Enum;


namespace Services.Model
{
    public class CourseModel
    {
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public decimal Price { get; set; }
        
    }
    public class CourseResponse
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
    }
}
