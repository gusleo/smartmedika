using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using System.Linq;
using dna.core.data.Infrastructure;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dna.core.data.Entities;
using System.Collections.Generic;
using dna.core.auth.Entity;
using dna.core.data.Extensions;

namespace MediCore.Data.Repositories
{

    public class HospitalAppointmentRatingRepository : EntityReadWriteBaseRepository<HospitalAppointmentRating>, IHospitalAppointmentRatingRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public HospitalAppointmentRatingRepository(IMediCoreContext context) : base(context)
        {
        }

        public async Task<PaginationEntity<HospitalAppointmentRating>>GetHospitalRatingAsync(int hospitalId, int pageIndex, int pageSize)
        {
            var result = (from r in MediCoreContext.HospitalAppointmentRating
                          join ha in MediCoreContext.HospitalAppointment on r.HospitalAppointmentId equals ha.Id
                          where ha.HospitalId == hospitalId
                          select r);
            return new PaginationEntity<HospitalAppointmentRating>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public async Task<PaginationEntity<HospitalAppointmentRating>> GetStaffRatingAsync(int staffId, int pageIndex, int pageSize)
        {
          
            var result = (from r in MediCoreContext.HospitalAppointmentRating
                          join ha in MediCoreContext.HospitalAppointment on r.HospitalAppointmentId equals ha.Id
                          join us in MediCoreContext.Users on r.CreatedById equals us.Id
                          join ud in MediCoreContext.UserDetail on us.Id equals ud.UserId
                          where ha.MedicalStaffId == staffId
                          select new HospitalAppointmentRating{
                              Id = r.Id,
                              Rating = r.Rating,
                              Testimoni = r.Testimoni,
                              CreatedDate = r.CreatedDate,
                              HospitalAppointmentId = r.HospitalAppointmentId,
                              CreatedById = r.CreatedById,
                              CreatedByUser = new ApplicationUser{
                                Id = ud == null ? 0 :  ud.UserId,
                                FirstName = ud.FirstName,
                                LastName = ud.LastName
                              }
                          });
            Console.Write(result.ToSql());
            return new PaginationEntity<HospitalAppointmentRating>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };

        }

        public async Task<PaginationEntity<HospitalAppointmentRating>> GetUserRatingAsync(int userId, int pageIndex, int pageSize)
        {
            var result = (from r in MediCoreContext.HospitalAppointmentRating
                          join ha in MediCoreContext.HospitalAppointment on r.HospitalAppointmentId equals ha.Id
                          join h in MediCoreContext.Hospital on ha.HospitalId equals h.Id
                          where ha.UserId == userId
                          select new HospitalAppointmentRating
                          {
                              Id = r.Id,
                              Rating = r.Rating,
                              Testimoni = r.Testimoni,
                              CreatedDate = r.CreatedDate,
                              HospitalAppointmentId = r.HospitalAppointmentId,
                              HospitalAppointment = new HospitalAppointment
                              {
                                  Hospital = new Hospital {  Id = h.Id, Name = h.Name },
                                  MedicalStaff = (from s in MediCoreContext.MedicalStaff 
                                                  where s.Id == ha.MedicalStaffId
                                                  select new MedicalStaff
                                                  {
                                                      Id = s.Id,
                                                      Title = s.Title,
                                                      FirstName = s.FirstName,
                                                      LastName = s.LastName,
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
                                                  }).FirstOrDefault()
                              },
                              
                          });
            return new PaginationEntity<HospitalAppointmentRating>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public async Task<IList<HospitalAppointmentRating>> GetTotalHospitalRatingAsync(int hospitalId)
        {
            var result = (from r in MediCoreContext.HospitalAppointmentRating
                          join ha in MediCoreContext.HospitalAppointment on r.HospitalAppointmentId equals ha.Id
                          where ha.HospitalId == hospitalId
                          select new HospitalAppointmentRating { Rating = r.Rating });
            return await result.ToListAsync();
        }

        public async Task<IList<HospitalAppointmentRating>> GetTotalStaffRatingAsync(int staffId)
        {
            var result = (from r in MediCoreContext.HospitalAppointmentRating
                          join ha in MediCoreContext.HospitalAppointment on r.HospitalAppointmentId equals ha.Id
                          where ha.MedicalStaffId == staffId
                          select new HospitalAppointmentRating { Rating = r.Rating });
            return await result.ToListAsync();
        }
    }
}
