using dna.core.data.Infrastructure;
using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using dna.core.data.Entities;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace MediCore.Data.Repositories
{
    public class MedicalStaffRepository : EntityReadWriteBaseRepository<MedicalStaff>, IMedicalStaffRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public MedicalStaffRepository(IMediCoreContext context) : base(context)
        {
        }

        
        public async Task<PaginationEntity<MedicalStaff>> GetAllStaffByHospitalAndTypeAsync(int hospitalId, MedicalStaffType[] types, MedicalStaffStatus[] status, int pageIndex, int pageSize, bool includeOperatingHours = false, string clue = "")
        {
            if ( !String.IsNullOrEmpty(clue) )
                clue = clue.ToLower();

            var result = (from m in MediCoreContext.MedicalStaff
                          join hm in MediCoreContext.HospitalMedicalStaff on m.Id equals hm.MedicalStaffId
                          where hm.HospitalId == hospitalId && types.Contains(m.StaffType) == true && status.Contains(m.Status)
                          && hm.Status == HospitalStaffStatus.Active
                          && (String.IsNullOrWhiteSpace(clue) == true || (m.FirstName.ToLower().Contains(clue) || m.LastName.ToLower().Contains(clue)))
                          orderby m.UpdatedDate descending
                          select new MedicalStaff {
                              Id = m.Id,
                              CreatedById = m.CreatedById,
                              AssociatedToUserId = m.CreatedById,
                              UpdatedById = m.UpdatedById,
                              CreatedDate = m.CreatedDate,
                              UpdatedDate = m.UpdatedDate,
                              Email = m.Email,
                              FirstName = m.FirstName,
                              LastName = m.LastName,
                              MedicalStaffRegisteredNumber = m.MedicalStaffRegisteredNumber,
                              PrimaryPhone = m.PrimaryPhone,
                              SecondaryPhone = m.SecondaryPhone,
                              Title = m.Title,
                              StaffType = m.StaffType,
                              Status = m.Status,
                              Rating = m.Rating,
                              Images = (from i in MediCoreContext.Image
                                        join map in MediCoreContext.MedicalStaffImage on i.Id equals map.ImageId
                                        where map.MedicalStaffId == m.Id && i.IsPrimary == true
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
                              MedicalStaffSpecialists = (from s in MediCoreContext.MedicalStaffSpecialist
                                                         join map in MediCoreContext.MedicalStaffSpecialistMap on s.Id equals map.MedicalStaffSpecialistId
                                                         where map.MedicalStaffId == m.Id
                                                         select new MedicalStaffSpecialistMap()
                                                         {
                                                             Id = map.Id,
                                                             MedicalStaffSpecialist = new MedicalStaffSpecialist()
                                                             {
                                                                 Id = s.Id,
                                                                 Name = s.Name,
                                                                 Alias = s.Alias
                                                             }
                                                         }).ToList(),
                              MedicalStaffClinics = includeOperatingHours == false 
                                                        ? new List<HospitalMedicalStaff>()
                                                        : (from ms in MediCoreContext.HospitalMedicalStaff                                                          
                                                           where ms.Id == hm.Id                                                          
                                                           select new HospitalMedicalStaff()
                                                           {
                                                               Id = ms.Id,
                                                               OperatingHours = (from op in MediCoreContext.StaffOperatingHours
                                                                                 where op.HospitalMedicalStaffId == ms.Id
                                                                                 orderby op.Day ascending
                                                                                 select op).ToList()
                                                           }).ToList()
                              });



            return new PaginationEntity<MedicalStaff>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
        public async Task<PaginationEntity<MedicalStaff>> GetAllStaffByTypeAsync(MedicalStaffType[] types, int pageIndex, int pageSize, string clue = "")
        {
            if ( !String.IsNullOrEmpty(clue) )
                clue = clue.ToLower();

            var result = (from m in MediCoreContext.MedicalStaff                          
                          where types.Contains(m.StaffType) == true 
                          && (String.IsNullOrWhiteSpace(clue) == true || (m.FirstName.ToLower().Contains(clue) || m.LastName.ToLower().Contains(clue)))
                          let medicalStaffSpecialist = (from s in MediCoreContext.MedicalStaffSpecialist
                                                        join map in MediCoreContext.MedicalStaffSpecialistMap on s.Id equals map.MedicalStaffSpecialistId
                                                        where map.MedicalStaffId == m.Id
                                                        select new MedicalStaffSpecialistMap()
                                                        {
                                                            Id = map.Id,
                                                            MedicalStaffSpecialist = new MedicalStaffSpecialist()
                                                            {
                                                                Id = s.Id,
                                                                Name = s.Name,
                                                                Alias = s.Alias
                                                            }
                                                        })
                          orderby m.UpdatedDate descending
                          select new MedicalStaff
                          {
                              Id = m.Id,
                              CreatedById = m.CreatedById,
                              AssociatedToUserId = m.CreatedById,
                              UpdatedById = m.UpdatedById,
                              CreatedDate = m.CreatedDate,
                              UpdatedDate = m.UpdatedDate,
                              Email = m.Email,
                              FirstName = m.FirstName,
                              LastName = m.LastName,
                              MedicalStaffRegisteredNumber = m.MedicalStaffRegisteredNumber,
                              PrimaryPhone = m.PrimaryPhone,
                              SecondaryPhone = m.SecondaryPhone,
                              Title = m.Title,
                              StaffType = m.StaffType,
                              Status = m.Status,
                              Rating = m.Rating,
                              MedicalStaffSpecialists = medicalStaffSpecialist.ToList(),
                          });



            return new PaginationEntity<MedicalStaff>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).OrderByDescending(x => x.UpdatedDate).ToListAsync()
            };
        }
        public async Task<MedicalStaff> GetStaffWithHospitalDetailAsync(int id, bool includeHospital = true)
        {
            var result = await (from m in MediCoreContext.MedicalStaff
                          where m.Id == id
                          select new MedicalStaff() {
                              Id = m.Id,
                              AssociatedToUserId = m.AssociatedToUserId,
                              FirstName = m.FirstName,
                              LastName = m.LastName,
                              Title = m.Title,
                              MedicalStaffRegisteredNumber = m.MedicalStaffRegisteredNumber,
                              PrimaryPhone = m.PrimaryPhone,
                              SecondaryPhone = m.SecondaryPhone,
                              StaffType = m.StaffType,
                              Status = m.Status,
                              Rating = m.Rating,
                              Images = (from i in MediCoreContext.Image
                                        join map in MediCoreContext.MedicalStaffImage on i.Id equals map.ImageId
                                        where map.MedicalStaffId == m.Id &&  i.IsPrimary == true
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
                              MedicalStaffSpecialists = (from s in MediCoreContext.MedicalStaffSpecialist
                                                         join map in MediCoreContext.MedicalStaffSpecialistMap on s.Id equals map.MedicalStaffSpecialistId
                                                         where map.MedicalStaffId == m.Id
                                                         select new MedicalStaffSpecialistMap()
                                                         {
                                                             Id = map.Id,
                                                             MedicalStaffSpecialist = new MedicalStaffSpecialist()
                                                             {
                                                                 Id = s.Id,
                                                                 Name = s.Name,
                                                                 Alias = s.Alias
                                                             }
                                                         }).ToList(),
                              MedicalStaffClinics = includeHospital == false ? 
                                                        new List<HospitalMedicalStaff>() : 
                                                    (from hs in MediCoreContext.HospitalMedicalStaff
                                                     join h in MediCoreContext.Hospital on hs.HospitalId equals h.Id
                                                     join r in MediCoreContext.Regency on h.RegencyId equals r.Id
                                                     join reg in MediCoreContext.Region on r.RegionId equals reg.Id
                                                     where hs.MedicalStaffId == id
                                                     select new HospitalMedicalStaff()
                                                     {
                                                         Id = hs.Id,
                                                         OperatingHours = (from op in MediCoreContext.StaffOperatingHours
                                                                           where op.HospitalMedicalStaffId == hs.Id
                                                                           select op).ToList(),
                                                         Hospital = new Hospital()
                                                         {
                                                             Id = h.Id,
                                                             Name = h.Name,
                                                             Longitude = h.Longitude,
                                                             Latitude = h.Latitude,
                                                             Address = h.Address,
                                                             Regency = new Regency{
                                                                Id = r.Id,
                                                                Name = r.Name,
                                                                Region = new Region{
                                                                    Id = reg.Id,
                                                                    Name = reg.Name
                                                                }
                                                             }

                                                         }
                                                     }).ToList()

                          }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<PaginationEntity<MedicalStaff>> GetAllStaffOrderByDistanceAsync(double longitude, double latitude, int radius, string search, int[] excludeStaffIds, MedicalStaffType[] types, MedicalStaffStatus[] status, int pageIndex, int pageSize)
        {
            
            var result = (from m in MediCoreContext.MedicalStaff 
                          join r in MediCoreContext.Regency on m.RegencyId equals r.Id
                          let geo = new GeoCoordinate { Latitude = r.Latitude, Longitude = r.Longitude }
                          where (m.FirstName + ' ' + m.LastName).ToLower().Contains(search.ToLower()) && types.Contains(m.StaffType) == true 
                          && status.Contains(m.Status) && !(excludeStaffIds.Any(p => p == m.Id))
                          let medicalStaffSpecialist = (from s in MediCoreContext.MedicalStaffSpecialist
                                                        join map in MediCoreContext.MedicalStaffSpecialistMap on s.Id equals map.MedicalStaffSpecialistId
                                                        where map.MedicalStaffId == m.Id
                                                        select new MedicalStaffSpecialistMap()
                                                        {
                                                            Id = map.Id,
                                                            MedicalStaffSpecialist = new MedicalStaffSpecialist()
                                                            {
                                                                Id = s.Id,
                                                                Name = s.Name,
                                                                Alias = s.Alias
                                                            }
                                                        })
                          orderby geo.GetDistanceTo(new GeoCoordinate { Latitude = latitude, Longitude = longitude})
                          select new MedicalStaff
                          {
                              Id = m.Id,
                              CreatedById = m.CreatedById,
                              AssociatedToUserId = m.CreatedById,
                              UpdatedById = m.UpdatedById,
                              CreatedDate = m.CreatedDate,
                              UpdatedDate = m.UpdatedDate,
                              Email = m.Email,
                              FirstName = m.FirstName,
                              LastName = m.LastName,
                              MedicalStaffRegisteredNumber = m.MedicalStaffRegisteredNumber,
                              PrimaryPhone = m.PrimaryPhone,
                              SecondaryPhone = m.SecondaryPhone,
                              Title = m.Title,
                              StaffType = m.StaffType,
                              Status = m.Status,
                              Rating = m.Rating,
                              MedicalStaffSpecialists = medicalStaffSpecialist.ToList(),
                          });

            return new PaginationEntity<MedicalStaff>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public async Task<PaginationEntity<MedicalStaff>> FindNearestStaffReferenceByHospitalAsync(double longitude, double latitude, int radius,
                                                                                                   string search, MedicalStaffType type, List<int> polyClinicIds, int pageIndex, int pageSize)
        {
            if ( !String.IsNullOrEmpty(search) )
                search = search.ToLower();

            var hospitals = await (from h in MediCoreContext.Hospital
                             join p in MediCoreContext.PolyClinicToHospitalMap on h.Id equals p.HospitalId
                             let geo = new GeoCoordinate { Latitude = h.Latitude ?? 0, Longitude = h.Longitude ?? 0 }
                             orderby geo.GetDistanceTo(new GeoCoordinate { Latitude = latitude, Longitude = longitude })
                                   where h.Status == HospitalStatus.Active && ( polyClinicIds.Count == 0 || polyClinicIds.Contains(p.PolyClinicId) ) 
                                   && (geo.GetDistanceTo(new GeoCoordinate { Latitude = latitude, Longitude = longitude }) < radius)
                             select h.Id).ToListAsync();

            var result = (from m in MediCoreContext.MedicalStaff                         
                          join h in MediCoreContext.HospitalMedicalStaff on m.Id equals h.MedicalStaffId
                          join hos in MediCoreContext.Hospital on h.HospitalId equals hos.Id
                          join reg in MediCoreContext.Regency on hos.RegencyId equals reg.Id
                          let geo = new GeoCoordinate { Latitude = hos.Latitude ?? 0, Longitude = hos.Longitude ?? 0 }
                          where h.Status == HospitalStaffStatus.Active && m.Status == MedicalStaffStatus.Active 
                            && hospitals.Contains(h.HospitalId) && hos.Status == HospitalStatus.Active
                          && (String.IsNullOrWhiteSpace(search) == true || (m.FirstName.ToLower().Contains(search) || m.LastName.ToLower().Contains(search)))
                          select new MedicalStaff
                          {
                              Id = m.Id,
                              CreatedById = m.CreatedById,
                              AssociatedToUserId = m.CreatedById,
                              UpdatedById = m.UpdatedById,
                              CreatedDate = m.CreatedDate,
                              UpdatedDate = m.UpdatedDate,
                              Email = m.Email,
                              FirstName = m.FirstName,
                              LastName = m.LastName,
                              MedicalStaffRegisteredNumber = m.MedicalStaffRegisteredNumber,
                              PrimaryPhone = m.PrimaryPhone,
                              SecondaryPhone = m.SecondaryPhone,
                              Title = m.Title,
                              StaffType = m.StaffType,
                              Status = m.Status, 
                              Rating = m.Rating,
                              Regency = new Regency { Id = reg.Id, Name = reg.Name},
                              Distance = geo.GetDistanceTo(new GeoCoordinate { Latitude = latitude, Longitude = longitude }),
                              Images = (from i in MediCoreContext.Image
                                        join map in MediCoreContext.MedicalStaffImage on i.Id equals map.ImageId
                                        where map.MedicalStaffId == m.Id &&  i.IsPrimary == true
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
                              MedicalStaffSpecialists = (from s in MediCoreContext.MedicalStaffSpecialist
                                                         join map in MediCoreContext.MedicalStaffSpecialistMap on s.Id equals map.MedicalStaffSpecialistId
                                                         where map.MedicalStaffId == m.Id
                                                         select new MedicalStaffSpecialistMap()
                                                         {
                                                             Id = map.Id,
                                                             MedicalStaffSpecialist = new MedicalStaffSpecialist()
                                                             {
                                                                 Id = s.Id,
                                                                 Name = s.Name,
                                                                 Alias = s.Alias
                                                             }
                                                         }).ToList(),
                          });

            return new PaginationEntity<MedicalStaff>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await result.CountAsync(),
                Items = await result.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };

        }
        

    }


}
