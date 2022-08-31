using dna.core.data.Entities;
using dna.core.data.Infrastructure;
using dna.core.data.Repositories;
using GeoCoordinatePortable;
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
    public class HospitalRepository : EntityReadWriteBaseRepository<Hospital>, IHospitalRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public HospitalRepository(IMediCoreContext context) : base(context)
        {

        }
        public async Task<Hospital> GetHospitalDetailAsync(int id)
        {
            var query = from h in MediCoreContext.Hospital
                            .Include(r => r.OperatingHours)
                                .Include(x => x.PolyClinicMaps)
                                    .ThenInclude(y => y.PolyClinic)
                                .Include(x => x.Regency)
                                    .ThenInclude(y => y.Region)
                                        where h.Id == id
                                            select h;

            return await query.FirstOrDefaultAsync();
        }
        public async Task<PaginationEntity<Hospital>> FindHospitalByFilterStatusAsync(int regionId, string clue, HospitalStatus[] status, int pageIndex, int pageSize)
        {
           

            var query = (from h in MediCoreContext.Hospital
                         join reg in MediCoreContext.Regency on h.RegencyId equals reg.Id
                         join regi in MediCoreContext.Region on reg.RegionId equals regi.Id
                         join c in MediCoreContext.Country on regi.CountryId equals c.Id
                         where (regionId == 0 || regi.Id == regionId) && status.Contains(h.Status)
                         orderby h.Id descending
                         select new Hospital
                         {
                             Id = h.Id,
                             Name = h.Name,
                             Address = h.Address,
                             IsHaveAmbulance = h.IsHaveAmbulance,
                             ZipCode = h.ZipCode,
                             Status = h.Status,  
                             Rating = h.Rating,
                             Regency = new Regency
                             {
                                 Id = reg.Id,
                                 Name = reg.Name,
                                 Region = new Region
                                 {
                                     Id = regi.Id,
                                     Name = regi.Name,
                                     Country = new Country
                                     {
                                         Id = c.Id,
                                         Name = c.Name
                                     }
                                 }
                             }
                         });

            if ( !String.IsNullOrWhiteSpace(clue) ){
                clue = clue.ToLower();
                query = query.Where(x => x.Name.ToLower() == clue);
            }
                

            

            return new PaginationEntity<Hospital>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await query.CountAsync(),
                Items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }

        public async Task<IList<Hospital>> GetHospitalAssociatedToUserAsync(int userId)
        {
            IQueryable<Hospital> query;
            if(userId == 0 )
            {
                query = (from h in MediCoreContext.Hospital                        
                         select new Hospital
                         {
                             Id = h.Id,
                             Name = h.Name,
                             Status = h.Status
                         });
            }else
            {
                query = (from o in MediCoreContext.HospitalOperator
                         join h in MediCoreContext.Hospital on o.HospitalId equals h.Id
                         where o.UserId == userId
                         select new Hospital
                         {
                             Id = h.Id,
                             Name = h.Name,
                             Status = h.Status
                         });
            }


            return await query.ToListAsync();
        }

        public async Task<PaginationEntity<Hospital>> FindNearestHospitalAsync(double longitude, double latitude, int radius, List<int> polyClinicIds, int pageIndex, int pageSize, string clue = "")
        {
            int today = DateTime.UtcNow.Day;

            /**
             * This method modied because when join directly with
             * table "PolyClinicToHospitalMap" it will showing duplicate data
             * since, "PolyClinicToHospitalMap" is one to many relationship
             * To make sure no duplicate data, we distinct the hospital ids
             * This method need to check agan if we already have ton of data
            **/
            var hospitalIds = (from h in MediCoreContext.Hospital
                                     join p in MediCoreContext.PolyClinicToHospitalMap on h.Id equals p.HospitalId
                                     let geo = new GeoCoordinate { Latitude = h.Latitude ?? 0, Longitude = h.Longitude ?? 0 }
                                     orderby geo.GetDistanceTo(new GeoCoordinate { Latitude = latitude, Longitude = longitude })
                                     where h.Status == HospitalStatus.Active && (polyClinicIds.Count == 0 || polyClinicIds.Contains(p.PolyClinicId))
                                     && (String.IsNullOrEmpty(clue) == true || h.Name.ToLower().Contains(clue.ToLower()))
                                     && (geo.GetDistanceTo(new GeoCoordinate { Latitude = latitude, Longitude = longitude }) < radius)
                                     select h.Id);
            
            
            var hospitals = (from h in MediCoreContext.Hospital
                             join reg in MediCoreContext.Regency on h.RegencyId equals reg.Id
                             join regi in MediCoreContext.Region on reg.RegionId equals regi.Id
                             join c in MediCoreContext.Country on regi.CountryId equals c.Id
                             let geo = new GeoCoordinate { Latitude = h.Latitude ?? 0, Longitude = h.Longitude ?? 0 }
                             where hospitalIds.Contains( h.Id )
                             select new Hospital
                             {
                                 Id = h.Id,
                                 Address = h.Address,
                                 Description = h.Description,
                                 IsHaveAmbulance = h.IsHaveAmbulance,
                                 Name = h.Name,
                                 Longitude = h.Longitude,
                                 Latitude = h.Latitude,
                                 RegencyId = h.RegencyId,
                                 Rating = h.Rating,
                                 Distance = geo.GetDistanceTo(new GeoCoordinate { Latitude = latitude, Longitude = longitude }),
                                 Regency = new Regency
                                 {
                                     Id = reg.Id,
                                     Name = reg.Name,
                                     Region = new Region
                                     {
                                         Id = regi.Id,
                                         Name = regi.Name,
                                         Country = new Country
                                         {
                                             Id = c.Id,
                                             Name = c.Name
                                         }
                                     }
                                 },
                                 OperatingHours = (from o in MediCoreContext.HospitalOperatingHours
                                                   where o.HospitalId == h.Id && o.Day == today && o.IsClossed == false
                                                   select o).ToList(),
                                 Images = (from i in MediCoreContext.Image
                                            join map in MediCoreContext.HospitalImage on i.Id equals map.ImageId
                                            where i.IsPrimary && map.HospitalId == h.Id
                                            select new HospitalImage
                                            {
                                                Id = map.Id,
                                                HospitalId = map.HospitalId,
                                                ImageId = map.ImageId,
                                                Image = new Image
                                                {
                                                    Id = i.Id,
                                                    FileExtension = i.FileExtension,
                                                    FileName = i.FileName,
                                                    ImageUrl = i.ImageUrl,
                                                    IsPrimary = i.IsPrimary
                                                     
                                                }
                                            }).ToList()
                             });

            return new PaginationEntity<Hospital>
            {
                Page = pageIndex,
                PageSize = pageSize,
                TotalCount = await hospitals.CountAsync(),
                Items = await hospitals.OrderBy(x => x.Distance).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync()
            };
        }
    }

   
}
