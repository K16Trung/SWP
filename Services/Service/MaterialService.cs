using AutoMapper;
using Services.Commons;
using Services.Entity;
using Services.Model;
using Services.Service.Interface;

namespace Services.Service
{
    public class MaterialService : IMaterialServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IClaimsServices _claimsServices;
        public MaterialService(IUnitOfWork unitOfWork, IMapper mapper, IClaimsServices claimsServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _claimsServices = claimsServices;
        }

        public async Task<ResponseModel<string>> CreateMaterial(MaterialModel material)
        {


            var materialObj = _mapper.Map<Material>(material);

            await _unitOfWork.materialRepo.CreateAsync(materialObj);
            var isSuccess = await _unitOfWork.SaveChangeAsync();
            if (isSuccess > 0)
            {
                return new ResponseModel<string> { Data = "Success" };
            }

            return new ResponseModel<string> { Errors = "Fail!" };
        }

        public async Task<ResponseModel<Pagination<MaterialModel>>> GetAllMaterial(int pageIndex = 1, int pageSize = 10)
        {
            var materialObj = await _unitOfWork.materialRepo.GetAllMaterialDetail(pageIndex, pageSize);
            var result = _mapper.Map<Pagination<MaterialModel>>(materialObj);
            if (materialObj.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<MaterialModel>> { Errors = "Not found" };
            }
            return new ResponseModel<Pagination<MaterialModel>> { Data = result };
        }

        public async Task<ResponseModel<Pagination<MaterialModel>>> GetMaterialById(int id, int pageIndex = 1, int pageSize = 10)
        {
            var materialObj = await _unitOfWork.materialRepo.GetMaterialDetailById(id, pageIndex, pageSize);
            var result = _mapper.Map<Pagination<MaterialModel>>(materialObj);
            if (materialObj.Items.Count() < 1)
            {
                return new ResponseModel<Pagination<MaterialModel>> { Errors = "Not found" };
            }
            return new ResponseModel<Pagination<MaterialModel>> { Data = result };
        }

        public async Task<ResponseModel<string>> RemoveMaterial(int id)
        {
            var materialObj = await _unitOfWork.materialRepo.GetEntityByIdAsync(id);
            if (materialObj is not null)
            {
                _unitOfWork.materialRepo.DeleteAsync(materialObj);
                var isSuccess = await _unitOfWork.SaveChangeAsync();
                if (isSuccess > 0)
                {
                    return new ResponseModel<string> { Data = "Success" };
                }
            }
            return new ResponseModel<string> { Errors = "Fail!" };
        }

        public async Task<ResponseModel<string>> UpdateMaterial(int id, MaterialResponse material)
        {
            var materialObj = await _unitOfWork.materialRepo.GetEntityByIdAsync(id);
            if (materialObj is not null)
            {
                _mapper.Map(material, materialObj);
                _unitOfWork.materialRepo.UpdateAsync(materialObj);
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
