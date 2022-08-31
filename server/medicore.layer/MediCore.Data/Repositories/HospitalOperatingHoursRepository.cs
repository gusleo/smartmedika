using dna.core.data.Repositories;
using dna.core.data.Repositories.Abstract;
using MediCore.Data;
using MediCore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories
{
    public class HospitalOperatingHoursRepository : EntityReadWriteBaseRepository<HospitalOperatingHours>, IHospitalOperatingHoursRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public HospitalOperatingHoursRepository(IMediCoreContext context) : base(context)
        {

        }
    }

    public interface IHospitalOperatingHoursRepository : IWriteBaseRepository<HospitalOperatingHours>, IReadBaseRepository<HospitalOperatingHours>
    {

    }
}
