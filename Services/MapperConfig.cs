using AutoMapper;
using Services.Commons;
using Services.Entity;
using Services.Model;

namespace Services
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<User, LoginModel>().ReverseMap();
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<Course, CourseModel>().ReverseMap();
            CreateMap<Course, CourseResponse>().ReverseMap();
            CreateMap<Post, PostModel>().ReverseMap();
            CreateMap<Post, PostResponse>().ReverseMap();
            CreateMap<Material, MaterialModel>().ReverseMap();
            CreateMap<Material, MaterialResponse>().ReverseMap();
            CreateMap<Order, OrderModel>()
                     .ForMember(dst => dst.Details, src => src.Ignore())
                     .ReverseMap();
            CreateMap<Order, OrderResponse>()
                .ForMember(dst => dst.UserName, src => src.Ignore())
                .ForMember(dst => dst.Details, src => src.Ignore())
                .ReverseMap()
                .ForMember(dst => dst.OrderStatus, src => src.Ignore());
            CreateMap<OrderDetail, OrderDetailModel>()
                .ReverseMap()
                .ForMember(dst => dst.OrderId, src => src.Ignore());
            CreateMap<OrderDetail, OrderDetailViewModel>()
                .ForMember(dst => dst.CourseName, src => src.Ignore())
                .ForMember(dst => dst.CoursePrice, src => src.Ignore())
                .ReverseMap();
            CreateMap<Payment, PaymentModel>()
                .ForMember(dst => dst.CVV, src => src.Ignore())
                .ForMember(dst => dst.CourseIds, src => src.Ignore())
                .ForMember(dst => dst.CardNumber, src => src.Ignore())
                .ForMember(dst => dst.CardDueDate, src => src.Ignore())
                .ForMember(dst => dst.CardName, src => src.Ignore())
                .ReverseMap()
                .ForMember(dst => dst.PaymentDate, src => src.Ignore());
            CreateMap<Feedback, FeedbackModel>()
                .ReverseMap();
            CreateMap<Feedback, FeedbackResponse>()
                .ForMember(dst => dst.CourseName, src => src.Ignore())
                .ForMember(dst => dst.UserName, src => src.Ignore())
                .ReverseMap();


        }
    }
}
