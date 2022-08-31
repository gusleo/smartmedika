using dna.core.data.Repositories;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories
{
    public class PatientRepository : EntityReadWriteBaseRepository<Patient>, IPatientRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public PatientRepository(IMediCoreContext context) : base(context)
        {
        }

        public bool AllowedUserToEdit(int id, int userId)
        {
            var result = (from p in MediCoreContext.Patient
                          where p.Id == id && p.CreatedById == userId
                          select p.Id).FirstOrDefault();
            return result > 0 ? true : false;
        }
    }

   
}
