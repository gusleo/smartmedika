using Dna.Core.Base.Data;
using MediCore.Lamda.Data.Entitties;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCore.Lamda.Data.Repositories
{
    public class MedicalStaffRepository : EntityReadWriteBaseRepository<MedicalStaff>, IMedicalStaffRepository
    {
        public IMediCoreContext Context
        {
            get { return _context as IMediCoreContext; }
        }
        public MedicalStaffRepository(IMediCoreContext context) : base(context)
        {
        }

        

       
    }

    public interface IMedicalStaffRepository : IReadBaseRepository<MedicalStaff>, IWriteBaseRepository<MedicalStaff>
    {
     
    }

  
}
