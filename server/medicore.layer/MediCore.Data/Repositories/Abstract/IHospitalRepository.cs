using dna.core.data.Infrastructure;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories.Abstract
{
    public interface IHospitalRepository : IReadBaseRepository<Hospital>, IWriteBaseRepository<Hospital>
    {
        /// <summary>
        /// Get hospital detail
        /// </summary>
        /// <remarks>Hospital detail with HospitalMeta & PolyClinic</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Hospital> GetHospitalDetailAsync(int id);

        /// <summary>
        /// Finds the hospital by filter status async.
        /// </summary>
        /// <returns>The hospital by filter status async.</returns>
        /// <param name="regionId">Region identifier.</param>
        /// <param name="clue">Clue.</param>
        /// <param name="status">Status.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        Task<PaginationEntity<Hospital>> FindHospitalByFilterStatusAsync(int regionId, string clue, HospitalStatus[] status, int pageIndex, int pageSize);
        /// <summary>
        /// Get all hospital asscoated to user
        /// </summary>
        /// <param name="userId">UserId = 0 if superadmin</param>
        /// <returns></returns>
        Task<IList<Hospital>> GetHospitalAssociatedToUserAsync(int userId);

        /// <summary>
        /// Finds the nearest hospital async.
        /// </summary>
        /// <returns>The nearest hospital async.</returns>
        /// <param name="longitude">Longitude.</param>
        /// <param name="latitude">Latitude.</param>
        /// <param name="radius">Radius.</param>
        /// <param name="polyClinicIds">Poly clinic identifiers.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="clue">Clue.</param>
        Task<PaginationEntity<Hospital>> FindNearestHospitalAsync(double longitude, double latitude, int radius, List<int> polyClinicIds, int pageIndex, int pageSize, string clue = "");
    }
}
