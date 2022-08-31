using dna.core.auth.Entity;
using dna.core.data.Infrastructure;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories.Abstract
{
    public interface IHospitalOperatorRepository : IWriteBaseRepository<HospitalOperator>, IReadBaseRepository<HospitalOperator>
    {
        Task<PaginationEntity<HospitalOperator>> GetHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, HospitalStaffStatus[] status = null, string clue = "");
        Task<PaginationEntity<ApplicationUser>> GetNonHospitalOperatorAsync(int hospitalId, int pageIndex, int pageSize, HospitalStaffStatus[] status = null, string clue = "");
        Task<IList<HospitalOperator>> GetUserHospitalAsync(int userId);
    }
}
