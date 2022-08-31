using Dna.Core.Base.Data;
using MediCore.Lamda.Data.Entitties;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediCore.Lamda.Data.Repositories
{
    public class HospitalRepository : EntityReadWriteBaseRepository<Hospital>, IHospitalRepository
    {
        public IMediCoreContext Context
        {
            get { return _context as IMediCoreContext; }
        }
        public HospitalRepository(IMediCoreContext context) : base(context)
        {
        }




    }

    public interface IHospitalRepository : IReadBaseRepository<Hospital>, IWriteBaseRepository<Hospital>
    {

    }
}
