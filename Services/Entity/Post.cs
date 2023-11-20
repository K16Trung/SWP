namespace Services.Entity
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
