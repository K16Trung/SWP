using AutoMapper;
using Services.Commons;
using Services.Entity;
using Services.Model;
using Services.Service.Interface;

namespace Services.Service
{
    public class PostService : IPostServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JWTSection _jwtSection;
        private readonly IMapper _mapper;
        private readonly IClaimsServices _claimsServices;
        public PostService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IClaimsServices claimsServices,
            JWTSection jwtSection)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsServices = claimsServices;
            _jwtSection = jwtSection;
        }

        public async Task<ResponseModel<PostResponse>> CreatePost(PostModel post)
        {

            var user = _claimsServices.GetCurrentUser();
            if (user is null)
            {
                return new ResponseModel<PostResponse>
                {
                    Errors = "User not logged in."
                };
            }
            var postObj = _mapper.Map<Post>(post);
            postObj.UserId = int.Parse(user);
           
            await _unitOfWork.postRepo.CreateAsync(postObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync();
            if (isSuccess > 0)
            {
                var result = _mapper.Map<PostResponse>(postObj);

                return new ResponseModel<PostResponse>
                {
                    Data = result
                };
            }
            return new ResponseModel<PostResponse> { Errors = "Create Course failed." };
        }

        public async Task<ResponseModel<Pagination<PostModel>>> GetAllPost(int pageIndex = 1, int pageSize = 10)
        {
            var postObj = await _unitOfWork.postRepo.GetAllPostDetail(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<PostModel>>(postObj);
            if (postObj.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<PostModel>> { Errors = "Not found" };
            }
            return new ResponseModel<Pagination<PostModel>> { Data = result };
        }

        public async Task<ResponseModel<Pagination<PostModel>>> GetPostById(int id, int pageIndex = 1, int pageSize = 10)
        {
            var postObj = await _unitOfWork.postRepo.GetPostDetailById(id, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<PostModel>>(postObj);
            if (postObj.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<PostModel>> { Errors = "Not found" };
            }
            return new ResponseModel<Pagination<PostModel>> { Data = result };
        }

        public async Task<ResponseModel<string>> RemovePost(int id)
        {
            var postObj = await _unitOfWork.postRepo.GetEntityByIdAsync(id);
            if (postObj is not null)
            {
                _unitOfWork.postRepo.DeleteAsync(postObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    return new ResponseModel<string> { Data = "Success" };
                }
            }
            return new ResponseModel<string> { Errors = "Fail!" };
        }

        public async Task<ResponseModel<string>> UpdatePost(int id, PostModel postModel)
        {
            var postObj = await _unitOfWork.postRepo.GetEntityByIdAsync(id);
            if (postObj is not null)
            {
                _mapper.Map(postModel, postObj);
                _unitOfWork.postRepo.UpdateAsync(postObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    return new ResponseModel<string> { Data = "Success" };
                }
            }
            return new ResponseModel<string> { Errors = "Fail!" };
        }
    }
}
