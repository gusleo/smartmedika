using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Data.Repositories
{
    public class HospitalStaffOperatingHoursRepository : EntityReadWriteBaseRepository<HospitalStaffOperatingHours>, IHospitalStaffOperatingHoursRepository
    {

        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }

        public HospitalStaffOperatingHoursRepository(IMediCoreContext context) : base(context)
        {

        }


        public async Task<List<HospitalMedicalStaff>> GetStaffOperatingHoursByDateAsync(int staffId, int dayOfWeek)
        {
            
            var result = (from s in MediCoreContext.MedicalStaff
                          join hs in MediCoreContext.HospitalMedicalStaff on s.Id equals hs.MedicalStaffId
                          join h in MediCoreContext.Hospital on hs.HospitalId equals h.Id
                          where s.Id == staffId
                          select new HospitalMedicalStaff()
                          {
                              Id = hs.Id,
                              OperatingHours = (from soh in MediCoreContext.StaffOperatingHours
                                                where
                                    soh.HospitalMedicalStaffId == hs.Id && soh.Day == dayOfWeek
                                                select soh).ToList(),
                              Hospital = new Hospital()
                              {
                                  Id = h.Id,
                                  Name = h.Name
                              }

                          });
            return await result.ToListAsync();
        }
    }


}
