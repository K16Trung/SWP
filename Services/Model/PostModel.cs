using Services.Entity;

namespace Services.Model
{
    public class PostModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
    }
    public class PostResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public int UserId { get; set; }
    }
}
