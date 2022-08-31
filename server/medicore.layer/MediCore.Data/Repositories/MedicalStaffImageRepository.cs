using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories
{
    public class MedicalStaffImageRepository : EntityReadWriteBaseRepository<MedicalStaffImage>, IMedicalStaffImageRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public MedicalStaffImageRepository(IMediCoreContext context) : base(context)
        {

        }
    }
}
