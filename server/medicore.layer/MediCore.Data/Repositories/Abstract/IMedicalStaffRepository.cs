using dna.core.data.Infrastructure;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories
{
    public interface IMedicalStaffRepository : IReadBaseRepository<MedicalStaff>, IWriteBaseRepository<MedicalStaff>
    {
        /// <summary>
        /// Finds the nearest staff reference by hospital async.
        /// </summary>
        /// <returns>The nearest staff reference by hospital async.</returns>
        /// <param name="longitude">Longitude.</param>
        /// <param name="latitude">Latitude.</param>
        /// <param name="radius">Radius.</param>
        /// <param name="search">Search.</param>
        /// <param name="type">Type.</param>
        /// <param name="polyClinicIds">Poly clinic identifiers.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <param name="clue">Clue.</param>
        Task<PaginationEntity<MedicalStaff>> FindNearestStaffReferenceByHospitalAsync(double longitude, double latitude, int radius,
                                                                                      string search, MedicalStaffType type, List<int> polyClinicIds, int pageIndex, int pageSize);
        /// <summary>
        /// Get all docter by hospital
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="types"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<PaginationEntity<MedicalStaff>> GetAllStaffByHospitalAndTypeAsync(int hospitalId, MedicalStaffType[] types, MedicalStaffStatus[] status, int pageIndex, int pageSize, bool includeOperatingHours = false, string clue = "");
        /// <summary>
        /// Get staff detail with information of operating hours on hospital
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MedicalStaff> GetStaffWithHospitalDetailAsync(int id, bool includeHospital = true);

        /// <summary>
        /// Get all staff order by distance
        /// </summary>
        /// <param name="longitude">Longitude Coordinate</param>
        /// <param name="latitude">Latitude Coordinate</param>
        /// <param name="radius">Radius from input coordinate</param>
        /// <param name="search">Search keyword</param>
        /// <param name="excludeStaffIds">Exclude staff ids</param>
        /// <param name="types">Type of Staff <see cref="MedicalStaffType"/></param>
        /// <param name="status">Status of Staff <see cref="MedicalStaffStatus"/></param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Width</param>
        /// <returns></returns>
        Task<PaginationEntity<MedicalStaff>> GetAllStaffOrderByDistanceAsync(double longitude, double latitude, int radius,
           string search, int[] excludeStaffIds, MedicalStaffType[] types, MedicalStaffStatus[] status, int pageIndex, int pageSize);

        /// <summary>
        /// Get staff by type
        /// </summary>
        /// <param name="types"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="clue"></param>
        /// <returns></returns>
        Task<PaginationEntity<MedicalStaff>> GetAllStaffByTypeAsync(MedicalStaffType[] types, int pageIndex, int pageSize, string clue = "");

        
    }
}
