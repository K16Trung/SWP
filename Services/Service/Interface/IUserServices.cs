using Services.Commons;
using Services.Model;

namespace Services.Service.Interface
{
    public interface IUserServices
    {
        Task<ResponseModel<string>> Login(LoginModel loginModel);
        Task<ResponseModel<string>> CreateUser(UserModel loginModel);
        Task<ResponseModel<Pagination<UserModel>>> GetUsers(int pageIndex = 0, int pageSize = 10);
        Task<ResponseModel<string>> UpdateUser(int id, UserModel loginModel);
        Task<ResponseModel<string>> RemoveUser(int id);


    }
}
