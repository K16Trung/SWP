using Microsoft.Extensions.DependencyInjection;
using Services.Service;
using Services.Service.Interface;

namespace Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection CoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentTimeService, CurrentTimeService>();
            services.AddScoped<IUserServices, UserService>();
            services.AddScoped<ICourseServices, CourseServices>();
            services.AddScoped<IPostServices, PostService>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IMaterialServices, MaterialService>();
            services.AddScoped<IFeedbackService, FeedbackService>();

            return services;
        }
    }
}
