using dna.core.data.Repositories;
using dna.core.data.Repositories.Abstract;
using dna.core.libs.Extension;
using MediCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dna.core.data.Infrastructure;
using MediCore.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using dna.core.data.Entities;

namespace MediCore.Data.Repositories
{
    public class HospitalAppointmentRepository : EntityReadWriteBaseRepository<HospitalAppointment>, IHospitalAppointmentRepository
    {
        private IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }

        }

        public HospitalAppointmentRepository(IMediCoreContext context) : base(context)
        {

        }
        public async Task<int> GetMaxQueueNumberAsync(int hospitalId, int medicalStaffId, DateTime appointmentDate)
        {
            var start = appointmentDate;

            var end = appointmentDate;


            var query = (from c in MediCoreContext.HospitalAppointment
                         where c.HospitalId == hospitalId && c.MedicalStaffId == medicalStaffId && (c.AppointmentDate >= start.StartOfDay() && c.AppointmentDate <= end.EndOfDay())
                         select c.QueueNumber);
            int queueNumber = 0;
            try
            {
                queueNumber = await query.MaxAsync();
            }
            catch ( Exception ex )
            {
                Console.Write(ex.Message);
            }
            return queueNumber + 1;
        }



        public async Task<PaginationEntity<HospitalAppointment>> GetUserAppointmentAsync(int userId, AppointmentStatus[] filter, int pageIndex, int pageSize)
        {

            var query = (from ha in MediCoreContext.HospitalAppointment
                         join h in MediCoreContext.Hospital on ha.HospitalId equals h.Id
                         join hm in MediCoreContext.MedicalStaff on ha.MedicalStaffId equals hm.Id
                         where ha.CreatedById == userId && filter.ToList().Contains(ha.AppointmentStatus)
                         select new HospitalAppointment
                         {
                             Id = ha.Id,
                             CreatedById = ha.CreatedById,
                             AppointmentDate = ha.AppointmentDate,
                             AppointmentStatus = ha.AppointmentStatus,
                             CreatedDate = ha.CreatedDate,
                             QueueNumber = ha.QueueNumber,
                             MedicalStaff = new MedicalStaff
                             {
                                 Id = hm.Id,
                                 Title = hm.Title,
                                 FirstName = hm.FirstName,
                                 LastName = hm.LastName,
                                 Images = (from i in MediCoreContext.Image
                                           join map in MediCoreContext.MedicalStaffImage on i.Id equals map.ImageId
                                           where map.MedicalStaffId == hm.Id && i.IsPrimary == true
                                           select new MedicalStaffImage
                                           {
                                               Id = map.Id,
                                               Image = new Image
                                               {
                                                   Id = i.Id,
                                                   FileExtension = i.FileExtension,
                                                   FileName = i.FileName,
                                                   ImageUrl = i.ImageUrl
                                               }
                                           }).ToList(),
                                 MedicalStaffSpecialists = (from map in hm.MedicalStaffSpecialists
                                                            join sp in MediCoreContext.MedicalStaffSpecialist on map.MedicalStaffSpecialistId equals sp.Id
                                                            where map.MedicalStaffId == hm.Id
                                                            select new MedicalStaffSpecialistMap
                                                            {
                                                                Id = map.Id,
                                                                MedicalStaffSpecialist = new MedicalStaffSpecialist
                                                                {
                                                                    Id = sp.Id,
                                                                    Name = sp.Name,
                                                                    StaffType = sp.StaffType
                                                                }

                                                            }).ToList()
                             },
                             Hospital = new Hospital
                             {
                                 Id = h.Id,
                                 Name = h.Name
                             }
                         });
            return new PaginationEntity<HospitalAppointment>
            {
                Page = pageIndex,
                PageSize = pageSize,
                Items = await query.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(),
                TotalCount = await query.CountAsync()
            };
        }
        public async Task<PaginationEntity<HospitalAppointment>> GetHospitalStaffAppointmentAsync(int hospitalId, int staffId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, AppointmentStatus[] filter = null)
        {
            filter = filter ?? new AppointmentStatus[] { AppointmentStatus.Pending, AppointmentStatus.Process, AppointmentStatus.Finish, AppointmentStatus.Cancel };

            var query = (from ha in MediCoreContext.HospitalAppointment
                         join hm in MediCoreContext.MedicalStaff on ha.MedicalStaffId equals hm.Id
                         where ha.HospitalId == hospitalId && (staffId == 0 || ha.MedicalStaffId == staffId )
                         && (ha.AppointmentDate >= startDate.StartOfDay() && ha.AppointmentDate <= endDate.EndOfDay()) 
                         && filter.ToList().Contains(ha.AppointmentStatus) orderby ha.QueueNumber ascending
                         select new HospitalAppointment
                         {
                             Id = ha.Id,
                             CreatedById = ha.CreatedById,
                             AppointmentDate = ha.AppointmentDate,
                             AppointmentStatus = ha.AppointmentStatus,
                             CreatedDate = ha.CreatedDate,
                             QueueNumber = ha.QueueNumber,
                             PatientName = ha.PatientName,
                             PatientProblems = ha.PatientProblems,
                             MedicalStaff = staffId != 0 ? null : new MedicalStaff
                             {
                                 Id = hm.Id,
                                 Title = hm.Title,
                                 FirstName = hm.FirstName,
                                 LastName = hm.LastName
                             },
                             
                             // if not null patient then appointment is not registered user
                             // do not populate, else populate from patient db
                             AppointmentDetails = !String.IsNullOrWhiteSpace(ha.PatientName) ? null : 
                                                    ( from ad in MediCoreContext.HospitalAppointmentDetail
                                                        join p in MediCoreContext.Patient on ad.PatientId equals p.Id
                                                        where ha.Id == ad.HospitalAppointmentId
                                                        select new HospitalAppointmentDetail {
                                                             Id = ad.Id,
                                                             Problem = ad.Problem,
                                                             Patient = new Patient
                                                             {
                                                                 Id = p.Id,
                                                                 PatientName = p.PatientName
                                                             }
                                                    }).ToList(),
                         });

           
            return new PaginationEntity<HospitalAppointment>
            {
                Page = pageIndex,
                PageSize = pageSize,
                Items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(),
                TotalCount = await query.CountAsync()
            };
        }

        public async Task<HospitalAppointment> GetHospitalAppointmentDetailAsync(int id)
        {
            var query = (from ha in MediCoreContext.HospitalAppointment                         
                         join h in MediCoreContext.Hospital on ha.HospitalId equals h.Id
                         join r in MediCoreContext.Regency on h.RegencyId equals r.Id
                         join g in MediCoreContext.Region on r.RegionId equals g.Id
                         join s in MediCoreContext.MedicalStaff on ha.MedicalStaffId equals s.Id                         
                         where ha.Id == id
                         select new HospitalAppointment
                         {
                             Id = ha.Id,
                             HospitalId = ha.HospitalId,
                             MedicalStaffId = ha.MedicalStaffId,
                             AppointmentDate = ha.AppointmentDate,
                             AppointmentFinished = ha.AppointmentFinished,
                             QueueNumber = ha.QueueNumber, 
                             AppointmentStatus = ha.AppointmentStatus,
                             AppointmentDetails = (from ad in MediCoreContext.HospitalAppointmentDetail join pt in MediCoreContext.Patient on ad.PatientId equals pt.Id
                                                   where ad.HospitalAppointmentId == ha.Id 
                                                   select new HospitalAppointmentDetail
                                                   {
                                                        Id = ha.Id,
                                                        Problem = ad.Problem,
                                                        Patient = new Patient{
                                                            PatientName = pt.PatientName
                                                        }
                                                    }).ToList(),
                             Hospital = new Hospital
                             {
                                 Id = h.Id,
                                 Name = h.Name,
                                 Address = h.Address,
                                 PrimaryPhone = h.PrimaryPhone,
                                 Longitude = h.Longitude,
                                 Latitude = h.Latitude,
                                 Regency = new Regency
                                 {
                                     Id = r.Id,
                                     Name = r.Name,
                                     Region = new Region
                                     {
                                         Id = g.Id,
                                         Name = g.Name
                                     }
                                 }
                             },
                             MedicalStaff = new MedicalStaff
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
                                                  FileExtension = i.FileExtension,
                                                  FileName = i.FileName,
                                                  ImageUrl = i.ImageUrl
                                              }
                                          }).ToList(),
                                 MedicalStaffSpecialists = (from map in s.MedicalStaffSpecialists
                                                                join sp in MediCoreContext.MedicalStaffSpecialist on map.MedicalStaffSpecialistId equals sp.Id
                                                                where map.MedicalStaffId == s.Id
                                                                    select new MedicalStaffSpecialistMap {
                                                                        Id = map.Id,
                                                                        MedicalStaffSpecialist = new MedicalStaffSpecialist
                                                                        {
                                                                            Id = sp.Id,
                                                                            Name = sp.Name,
                                                                            StaffType = sp.StaffType
                                                                        }

                                                                    }).ToList()
                             },
                             
                         });
            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the queque estimate async without
        /// to much detail of information
        /// </summary>
        /// <returns>The queque estimate async.</returns>
        /// <param name="id">Identifier.</param>
        public async Task<HospitalAppointment> GetQuequeEstimateAsync(int id)
        {
            var query = (from ha in MediCoreContext.HospitalAppointment
                         join h in MediCoreContext.Hospital on ha.HospitalId equals h.Id
                         join s in MediCoreContext.MedicalStaff on ha.MedicalStaffId equals s.Id
                         where ha.Id == id
                         select new HospitalAppointment
                         {
                             Id = ha.Id,
                             HospitalId = ha.HospitalId,
                             MedicalStaffId = ha.MedicalStaffId,
                             AppointmentDate = ha.AppointmentDate,
                             QueueNumber = ha.QueueNumber,
                             CurrentQueueNumber = (from temp in MediCoreContext.HospitalAppointment
                                                    where temp.HospitalId == ha.HospitalId && temp.MedicalStaffId == ha.MedicalStaffId
                                                    && temp.AppointmentDate <= ha.AppointmentDate && temp.AppointmentStatus == AppointmentStatus.Process
                                                    select temp.QueueNumber).Max(),
                             Hospital = new Hospital
                             {
                                 Id = h.Id,
                                 Name = h.Name
                             },
                             MedicalStaff = new MedicalStaff
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
                                                   FileExtension = i.FileExtension,
                                                   FileName = i.FileName,
                                                   ImageUrl = i.ImageUrl
                                               }
                                           }).ToList(),
                                 MedicalStaffSpecialists = (from map in s.MedicalStaffSpecialists
                                                            join sp in MediCoreContext.MedicalStaffSpecialist on map.MedicalStaffSpecialistId equals sp.Id
                                                            where map.MedicalStaffId == s.Id
                                                            select new MedicalStaffSpecialistMap
                                                            {
                                                                Id = map.Id,
                                                                MedicalStaffSpecialist = new MedicalStaffSpecialist
                                                                {
                                                                    Id = sp.Id,
                                                                    Name = sp.Name,
                                                                    StaffType = sp.StaffType
                                                                }

                                                            }).ToList()
                             },

                         });
            return await query.FirstOrDefaultAsync();
        }

        public int GetHospitalAppointmentUserId(int id)
        {
            var result = (from i in MediCoreContext.HospitalAppointment
                          where i.Id == id
                          select i.UserId).FirstOrDefault();
            return result ?? 0;
        }

        public async Task<PaginationEntity<HospitalAppointment>> GetUserAppointmentNotRatedAsync(int userId, AppointmentStatus[] filter, int pageIndex, int pageSize)
        {
            var query = (from ha in MediCoreContext.HospitalAppointment
                         join h in MediCoreContext.Hospital on ha.HospitalId equals h.Id
                         join hm in MediCoreContext.MedicalStaff on ha.MedicalStaffId equals hm.Id
                         join r in MediCoreContext.HospitalAppointmentRating on ha.Id equals r.HospitalAppointmentId into rating
                         from r in rating.DefaultIfEmpty()
                         where ha.CreatedById == userId && filter.ToList().Contains(ha.AppointmentStatus) && r == null
                         select new HospitalAppointment
                         {
                             Id = ha.Id,
                             CreatedById = ha.CreatedById,
                             AppointmentDate = ha.AppointmentDate,
                             AppointmentStatus = ha.AppointmentStatus,
                             CreatedDate = ha.CreatedDate,
                             QueueNumber = ha.QueueNumber,
                             MedicalStaff = new MedicalStaff
                             {
                                 Id = hm.Id,
                                 Title = hm.Title,
                                 FirstName = hm.FirstName,
                                 LastName = hm.LastName,
                                 Images = (from i in MediCoreContext.Image
                                           join map in MediCoreContext.MedicalStaffImage on i.Id equals map.ImageId
                                           where map.MedicalStaffId == hm.Id && i.IsPrimary == true
                                           select new MedicalStaffImage
                                           {
                                               Id = map.Id,
                                               Image = new Image
                                               {
                                                   Id = i.Id,
                                                   FileExtension = i.FileExtension,
                                                   FileName = i.FileName,
                                                   ImageUrl = i.ImageUrl
                                               }
                                           }).ToList(),
                                 MedicalStaffSpecialists = (from map in hm.MedicalStaffSpecialists
                                                            join sp in MediCoreContext.MedicalStaffSpecialist on map.MedicalStaffSpecialistId equals sp.Id
                                                            where map.MedicalStaffId == hm.Id
                                                            select new MedicalStaffSpecialistMap
                                                            {
                                                                Id = map.Id,
                                                                MedicalStaffSpecialist = new MedicalStaffSpecialist
                                                                {
                                                                    Id = sp.Id,
                                                                    Name = sp.Name,
                                                                    StaffType = sp.StaffType
                                                                }

                                                            }).ToList()
                             },
                             Hospital = new Hospital
                             {
                                 Id = h.Id,
                                 Name = h.Name
                             }
                         });
            return new PaginationEntity<HospitalAppointment>
            {
                Page = pageIndex,
                PageSize = pageSize,
                Items = await query.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(),
                TotalCount = await query.CountAsync()
            };
        }
    }

    
}
