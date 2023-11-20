using Services.Commons;
using Services.Entity;

namespace Services.Repository.Interface
{
    public interface IPostRepo : IGenericRepo<Post>
    {
        Task<Pagination<Post>> GetAllPostDetail(int pageIndex = 1, int pageSize = 10);
        Task<Pagination<Post>> GetPostDetailById(int id, int pageIndex = 1, int pageSize = 10);
    }
}
