
using dna.core.data.Infrastructure;
using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories
{
    public class UserDetailRepository : EntityReadWriteBaseRepository<UserDetailMediCore>, Abstract.IUserDetailRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public UserDetailRepository(IMediCoreContext context) : base(context)
        {
        }

        public async Task<UserDetailMediCore> GetSingleDetail(int userId){
            var result = (from u in MediCoreContext.Users
                          join de in MediCoreContext.UserDetail on u.Id equals de.UserId into detail
                          from de in detail.DefaultIfEmpty()
                          join pa in MediCoreContext.Patient on u.Id equals pa.AssociatedUserId into patient
                          from pa in patient.DefaultIfEmpty()
                          where u.Id == userId
                          select new UserDetailMediCore
                          {
                              Id = de == null ? 0 : de.Id,
                              FirstName = de == null ? null : de.FirstName,
                              LastName = de == null ? null : de.LastName,
                              Patient = pa,
                              PatientId = de == null ? 0 : de.PatientId,
                              Address = de == null ? String.Empty : de.Address,
                              Avatar = de == null ? string.Empty : de.Avatar,
                              Latitude = de == null ? null : de.Latitude,
                              Longitude = de == null ? null : de.Longitude,
                              RegencyId = de == null ? null : de.RegencyId,
                              UserId = de == null ? 0 : de.UserId,
                              Regency = (de == null || de.RegencyId == null) ? null : 
                                            (from rg in MediCoreContext.Regency
                                               join r in MediCoreContext.Region on rg.RegionId equals r.Id
                                               where rg.Id == de.RegencyId
                                               select new Regency
                                               {
                                                   Id = rg.Id,
                                                   Region = new Region
                                                   {
                                                       Id = r.Id
                                                   }
                                           }).FirstOrDefault(),
                              User = new dna.core.auth.Entity.ApplicationUser
                              {
                                  Id = u.Id,
                                  Email = u.Email,
                                  PhoneNumber = u.PhoneNumber,
                                  EmailConfirmed = u.EmailConfirmed,
                                  PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                                  Status = u.Status,
                                  AccessFailedCount = u.AccessFailedCount
                              }
                          });
            return await result.FirstOrDefaultAsync();
        }
        public async Task<PaginationEntity<UserDetailMediCore>> GetAllWithDetailAsync(int pageIndex, int pageSize, string clue)
        {
           
            var result = (from u in MediCoreContext.Users
                          join de in MediCoreContext.UserDetail on u.Id equals de.UserId into detail
                          from de in detail.DefaultIfEmpty()
                          join pa in MediCoreContext.Patient on u.Id equals pa.AssociatedUserId into patient
                          from pa in patient.DefaultIfEmpty()
                          where (clue == String.Empty || u.Email.Contains(clue))
                          select new UserDetailMediCore
                          {
                              Id = de == null ? 0 : de.Id,
                              FirstName = de == null ? null : de.FirstName,
                              LastName = de == null ? null : de.LastName,
                              Patient = pa,
                              PatientId = de == null ? 0 : de.PatientId,
                              User = new dna.core.auth.Entity.ApplicationUser
                              {
                                  Id = u.Id,
                                  Email = u.Email,
                                  PhoneNumber = u.PhoneNumber,
                                  EmailConfirmed = u.EmailConfirmed,
                                  PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                                  Status = u.Status,
                                  AccessFailedCount = u.AccessFailedCount
                              }
                          });

            return new PaginationEntity<UserDetailMediCore>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };


           
        }
    }


}
