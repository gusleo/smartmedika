using dna.core.auth.Entity;
using dna.core.data.Infrastructure;
using dna.core.data.Repositories;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories
{
    public class HospitalOperatorRepository : EntityReadWriteBaseRepository<HospitalOperator>, IHospitalOperatorRepository
    {

        private IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }

        }

        public HospitalOperatorRepository(IMediCoreContext context) : base(context)
        {

        }
        public async Task<PaginationEntity<HospitalOperator>> GetHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, HospitalStaffStatus[] status = null, string clue = "")
        {
            status = status ?? new HospitalStaffStatus[] { HospitalStaffStatus.Active, HospitalStaffStatus.InActive, HospitalStaffStatus.Suspended };

            var result = (from ho in MediCoreContext.HospitalOperator
                          join u in MediCoreContext.Users on ho.UserId equals u.Id
                          join ud in MediCoreContext.UserDetail on u.Id equals ud.UserId into detail
                          from ud in detail.DefaultIfEmpty()
                          where ho.HospitalId == hospitalId && status.ToList().Contains(ho.Status) &&
                                (clue == String.Empty || u.Email.Contains(clue))
                          select new HospitalOperator {
                              Id = ho.Id,
                              HospitalId = ho.HospitalId,
                              UserId = u.Id,
                              Status = ho.Status,
                              CreatedById = ho.CreatedById,
                              UpdatedById = ho.UpdatedById,
                              User = new ApplicationUser
                              {
                                  Id = u.Id,
                                  Email = u.Email,
                                  NormalizedEmail = u.NormalizedEmail,
                                  PhoneNumber = u.PhoneNumber,
                                  FirstName = ud == null ? "" : ud.FirstName,
                                  LastName = ud == null ? "" : ud.LastName
                                  
                              }
                          });

            return new PaginationEntity<HospitalOperator>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
        public async Task<PaginationEntity<ApplicationUser>> GetNonHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, HospitalStaffStatus[] status = null, string clue = "")
        {
            status = status ?? new HospitalStaffStatus[] { HospitalStaffStatus.Active, HospitalStaffStatus.InActive, HospitalStaffStatus.Suspended };

            var result = (from u in MediCoreContext.Users
                          join ho in MediCoreContext.HospitalOperator on u.Id equals ho.UserId into staff
                          join ud in MediCoreContext.UserDetail on u.Id equals ud.UserId into detail
                          from ud in detail.DefaultIfEmpty()
                          from ho in staff.Where(ho => ho.HospitalId == hospitalId &&
                              status.ToList().Contains(ho.Status)).DefaultIfEmpty()
                          where
                                ho == null &&
                                    (clue == String.Empty || u.Email.Contains(clue))
                          select new ApplicationUser 
                            {
                                Id = u.Id,
                                Email = u.Email,
                                NormalizedEmail = u.NormalizedEmail,
                                PhoneNumber = u.PhoneNumber,
                                FirstName = ud == null ? "" : ud.FirstName,
                                LastName = ud == null ? "" : ud.LastName,
                                Status = u.Status
                             }
                          );

            return new PaginationEntity<ApplicationUser>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
        public async Task<IList<HospitalOperator>> GetUserHospitalAsync(int userId)
        {
            var result = from ho in MediCoreContext.HospitalOperator
                         join h in MediCoreContext.Hospital on ho.HospitalId equals h.Id                          
                         where ho.UserId == userId
                         select new HospitalOperator
                         {
                            Id = ho.Id,
                            Status = ho.Status,
                            HospitalId = ho.HospitalId,
                            UserId = ho.UserId,
                            Hospital = new Hospital{
                                Name = h.Name
                            }
                         };
            return await result.ToListAsync();
        }
    }

    
}
