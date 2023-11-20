using Services.Entity;

namespace Services.Repository
{
    public interface IUserRepo : IGenericRepo<User>
    {
        Task<User?> Login(string email, string password);
        Task<bool> ExistEmail(string email);
    }
}
