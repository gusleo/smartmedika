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
    public class HospitalMedicalStaffRepository : EntityReadWriteBaseRepository<HospitalMedicalStaff>, IHospitalMedicalStaffRepository
    {
     
        private IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
            
        }

        public HospitalMedicalStaffRepository(IMediCoreContext context): base(context)
        {

        }
        
    }

    
}
