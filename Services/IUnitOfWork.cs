

using Microsoft.EntityFrameworkCore.Storage;
using Services.Repository;
using Services.Repository.Interface;

namespace Services
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangeAsync();
        Task<IDbContextTransaction> BeginTransaction();
        public IUserRepo userRepo { get; }
        public ICourseRepo courseRepo { get; }
        public IPostRepo postRepo { get; }
        public IOrderRepo orderRepo { get; }
        public IOrderDetailRepo orderDetailRepo { get; }
        public IMaterialRepo materialRepo { get; }
        public IPaymentRepo paymentRepo { get; }
        public IFeedbackRepo feedbackRepo { get; }
        public ICourseEnrollRepo courseEnrollRepo { get; }


    }
}
