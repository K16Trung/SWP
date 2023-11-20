using Services.Entity;

namespace Services.Model
{
    
    public class MaterialModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public int CourseId { get; set; }
    }
    public class MaterialResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public int CourseId { get; set; }
    }

}
