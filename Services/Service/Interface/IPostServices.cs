using Services.Commons;
using Services.Model;

namespace Services.Service.Interface
{
    public interface IPostServices
    {
        Task<ResponseModel<PostResponse>> CreatePost(PostModel post);
        Task<ResponseModel<Pagination<PostModel>>> GetAllPost(int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<Pagination<PostModel>>> GetPostById(int id, int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<string>> UpdatePost(int id, PostModel postModel);
        Task<ResponseModel<string>> RemovePost(int id);
    }
}
