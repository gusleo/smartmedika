using System.Threading.Tasks;
using dna.core.data.Infrastructure;
using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using System.Linq;
using dna.core.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Data.Repositories
{
    public class MedicalStaffFavoriteRepository : EntityReadWriteBaseRepository<MedicalStaffFavorite>, IMedicalStaffFavoriteRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public MedicalStaffFavoriteRepository(IMediCoreContext context) : base(context)
        {
        }



        public async Task<PaginationEntity<MedicalStaffFavorite>> GetAllByUserAsync(int userId, int pageIndex, int pageSize)
        {
            var results = (from f in MediCoreContext.MedicalStaffFavorite
                           join s in MediCoreContext.MedicalStaff on f.MedicalStaffId equals s.Id
                           where f.UserId == userId
                           select new MedicalStaffFavorite {
                               Id = f.Id,
                               UserId = f.UserId,
                               MedicalStaff = new MedicalStaff
                               {
                                   Id = s.Id,
                                   Title = s.Title,
                                   FirstName = s.FirstName,
                                   LastName = s.LastName,
                                   MedicalStaffSpecialists = (from spe in MediCoreContext.MedicalStaffSpecialist
                                                              join map in MediCoreContext.MedicalStaffSpecialistMap on spe.Id equals map.MedicalStaffSpecialistId
                                                              where map.MedicalStaffId == s.Id
                                                              select new MedicalStaffSpecialistMap()
                                                              {
                                                                  Id = map.Id,
                                                                  MedicalStaffSpecialist = new MedicalStaffSpecialist()
                                                                  {
                                                                      Id = spe.Id,
                                                                      Name = spe.Name
                                                                  }
                                                              }).ToList(),
                                   Images = (from i in MediCoreContext.Image
                                                      join map in MediCoreContext.MedicalStaffImage on i.Id equals map.ImageId
                                                      where map.MedicalStaffId == s.Id && i.IsPrimary == true
                                                      select new MedicalStaffImage
                                                      {
                                                          Id = map.Id,
                                                          Image = new Image
                                                          {
                                                              Id = i.Id,
                                                             
                                                              ImageUrl = i.ImageUrl
                                                          }
                                                      }).ToList(),
                               }
                           });
            return new PaginationEntity<MedicalStaffFavorite>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await results.CountAsync(),
                Items = await results.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
    }
}
