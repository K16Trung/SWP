using Services.Entity;
using Services.Repository.Interface;
using Services.Service.Interface;
using Services.Service;
using Services.Commons;
using Microsoft.EntityFrameworkCore;

namespace Services.Repository
{
    public class MaterialRepo : GenericRepo<Material>, IMaterialRepo
    {
        private readonly AppDBContext _context;
        public MaterialRepo(AppDBContext context, ICurrentTimeService currentTime, IClaimsServices claimsServices) : base(context, currentTime, claimsServices)
        {
            _context = context;
        }

        public async Task<Pagination<Material>> GetMaterialDetailById(int id, int pageIndex = 1, int pageSize = 10)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }

            var totalMaterialCount = await _dbSet.CountAsync(x => x.Id == id);

            var material = await _dbSet
                .Where(x => x.Id == id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new Pagination<Material>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = totalMaterialCount,
                Items = material
            };

            return result;
        }

        public async Task<Pagination<Material>> GetAllMaterialDetail(int pageIndex = 1, int pageSize = 10)
        {
            var itemCount = await _context.Materials.CountAsync();
            if (pageIndex == 0)
            {
                pageIndex = 1;
            }
            var material = await _context.Materials.Skip((pageIndex - 1) * pageSize)
                                                 .Take(pageSize)
                                                 .AsNoTracking()
                                                 .ToListAsync();

            var result = new Pagination<Material>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItemCount = itemCount,
                Items = material
            };

            return result;
        }
    }
}
