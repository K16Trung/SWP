using Services.Commons;
using Services.Model;

namespace Services.Service.Interface
{
    public interface IMaterialServices
    {
        Task<ResponseModel<string>> CreateMaterial(MaterialModel material);
        Task<ResponseModel<Pagination<MaterialModel>>> GetAllMaterial(int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<Pagination<MaterialModel>>> GetMaterialById(int id, int pageIndex = 1, int pageSize = 10);
        Task<ResponseModel<string>> UpdateMaterial(int id, MaterialResponse material);
        Task<ResponseModel<string>> RemoveMaterial(int id);

    }
}
