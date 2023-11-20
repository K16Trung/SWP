using AutoMapper;
using Services.Commons;
using Services.Entity;
using Services.Enum;
using Services.Model;
using Services.Service.Interface;

namespace Services.Service
{
    public class UserService : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTSection _jwtSection;
        private readonly IMapper _mapper;
        private readonly IClaimsServices _claimsServices;
        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimsServices claimsServices,
            JWTSection jwtSection)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsServices = claimsServices;
            _jwtSection = jwtSection;
        }
        public async Task<ResponseModel<string>> CreateUser(UserModel loginModel)
        {
            var isExist = await _unitOfWork.userRepo.ExistEmail(loginModel.Email);
            if (!isExist)
            {
                var user = _mapper.Map<User>(loginModel);
                user.Role = Role.Customer;
                await _unitOfWork.userRepo.CreateAsync(user);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    return new ResponseModel<string> { Data = "success" };
                }
            }
            return new ResponseModel<string> { Errors = "Fail to create user." };
        }

        public async Task<ResponseModel<Pagination<UserModel>>> GetUsers(int pageIndex = 0, int pageSize = 10)
        {
            var listUser = await _unitOfWork.userRepo.ToPagination(pageIndex: pageIndex, pageSize: pageSize);
            var result = _mapper.Map<Pagination<UserModel>>(listUser);
            if (listUser is null) return new ResponseModel<Pagination<UserModel>> { Errors = "List is empty"! };
            return new ResponseModel<Pagination<UserModel>> { Data = result };
        }

        public async Task<ResponseModel<string>> Login(LoginModel loginModel)
        {
            var response = new ResponseModel<string>();
            var user = await _unitOfWork.userRepo.Login(loginModel.Email, loginModel.Password);
            if (user == null || user.IsDeleted)
            {
                response.Errors = "User not exsits or has been banned.";
                return response;
            }
            response.Data = Utils.GenerateJwtToken(user.Id.ToString(), user.Email, user.Role.ToString(), _jwtSection);

            return response;
        }

        public async Task<ResponseModel<string>> RemoveUser(int id)
        {
            var response = new ResponseModel<string>();
            var user = await _unitOfWork.userRepo.GetEntityByIdAsync(id);
            if (user == null || user.IsDeleted)
            {
                response.Errors = "User not exsits or has been banned.";
                return response;
            }
            _unitOfWork.userRepo.DeleteAsync(user);
            var result = await _unitOfWork.SaveChangeAsync();
            response.Data = result.ToString();
            return response;
        }

        public async Task<ResponseModel<string>> UpdateUser(int id, UserModel loginModel)
        {
            var currentUser = _claimsServices.GetCurrentUser();
            if (currentUser == null || currentUser != id.ToString() || id < 0)
            {
                return new ResponseModel<string>
                {
                    Errors = "User not login yet."
                };
            }
            var user = await _unitOfWork.userRepo.GetEntityByIdAsync(id);
            if (user == null || user.IsDeleted)
            {
                return new ResponseModel<string>
                {
                    Errors = "User not exsits or has been banned."
                };
            }
            user.Address = loginModel.Address;
            user.Email = loginModel.Email;
            user.Password = loginModel.Password;
            user.Phone = loginModel.Phone;
            _unitOfWork.userRepo.UpdateAsync(user);
            var result = await _unitOfWork.SaveChangeAsync();
            return new ResponseModel<string>
            {
                Data = result.ToString(),
            };
        }
    }
}