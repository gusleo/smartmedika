using dna.core.data.Repositories;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;

namespace MediCore.Data.Repositories
{
    public class PolyClinicRepository : EntityReadWriteBaseRepository<PolyClinic>, IPolyClinicRepository
    {
        private IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }

        }

        public PolyClinicRepository(IMediCoreContext context) : base(context)
        {

        }
    }
}

    
