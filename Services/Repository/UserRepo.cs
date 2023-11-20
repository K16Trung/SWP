using Microsoft.EntityFrameworkCore;
using Services.Entity;
using Services.Repository;
using Services.Service;
using Services.Service.Interface;

namespace Services.Repository
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        public UserRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
        }

        public Task<bool> ExistEmail(string email)
        {
            return _dbSet.AnyAsync(x => x.Email == email);
        }

        public Task<User?> Login(string email, string password)
        {
            return _dbSet.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }
    }
}
