using Services.Commons;
using Services.Entity;

namespace Services.Repository.Interface
{
    public interface IMaterialRepo : IGenericRepo<Material>
    {
        Task<Pagination<Material>> GetAllMaterialDetail(int pageIndex = 1, int pageSize = 10);
        Task<Pagination<Material>> GetMaterialDetailById(int id, int pageIndex = 1, int pageSize = 10);


    }
}
