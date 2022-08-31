using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface IPatientService : IReadWriteService<PatientModel, Patient> {
        /// <summary>
        /// Get user's patient
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="filter">Filter by <see cref="PatientStatus"/></param>
        /// <returns></returns>
        Task<Response<PaginationSet<PatientModel>>> GetUserPatientAsync(int pageIndex, int pageSize = Constant.PageSize, PatientStatus[] filter = null);
    }
}
