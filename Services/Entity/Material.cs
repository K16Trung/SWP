

namespace Services.Entity
{
    public class Material : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public Course Course { get; set; }
        public int CourseId { get; set; }
    }
}
