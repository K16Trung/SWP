using Services.Service.Interface;
using Services.Service;
using Services.Repository;
using System;
using Services.Repository.Interface;
using Microsoft.EntityFrameworkCore.Storage;

namespace Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        private readonly ICurrentTimeService _currentTimeService;
        private readonly IClaimsServices _claimsServices;
        public UnitOfWork(AppDBContext context,
            ICurrentTimeService currentTimeService,
            IClaimsServices claimsServices)
        {
            _context = context;
            _currentTimeService = currentTimeService;
            _claimsServices = claimsServices;
        }
        public IUserRepo userRepo => new UserRepo(_context, _currentTimeService, _claimsServices);
        public ICourseRepo courseRepo => new CourseRepo(_context, _currentTimeService, _claimsServices);
        public IPostRepo postRepo => new PostRepo(_context, _currentTimeService, _claimsServices);
        public IOrderRepo orderRepo => new OrderRepo(_context, _currentTimeService, _claimsServices);
        public IOrderDetailRepo orderDetailRepo => new OrderDetailRepo(_context, _currentTimeService, _claimsServices);
        public IPaymentRepo paymentRepo => new PaymentRepo(_context, _currentTimeService, _claimsServices);
        public IFeedbackRepo feedbackRepo => new FeedbackRepo(_context, _currentTimeService, _claimsServices);
        public ICourseEnrollRepo courseEnrollRepo => new CourseEnrollRepo(_context, _currentTimeService, _claimsServices);
        public IMaterialRepo materialRepo => new MaterialRepo(_context, _currentTimeService, _claimsServices);

        public Task<int> SaveChangeAsync()
        {
            return _context.SaveChangesAsync();
        }
        public Task<IDbContextTransaction> BeginTransaction()
        {
            return _context.Database.BeginTransactionAsync();
        }
    }
}
