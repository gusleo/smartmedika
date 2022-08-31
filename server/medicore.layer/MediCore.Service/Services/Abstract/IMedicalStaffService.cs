using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using MediCore.Service.Model.Custom;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface IMedicalStaffService : IReadWriteService<MedicalStaffModel, MedicalStaff>
    {


        /// <summary>
        /// Set operating hours for medical staff to hospital
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="staffId"></param>
        /// <param name="operatingHours"></param>
        /// <returns></returns>
        Task<Response<IList<HospitalStaffOperatingHoursModel>>> SetOperatingHoursAsync(int hospitalId, int staffId, int estimateTime, IList<HospitalStaffOperatingHoursModel> operatingHours);

        /// <summary>
        /// Assign staff to hospital
        /// </summary>
        /// <param name="id">Staff Id</param>
        /// <param name="hospitalId">Hospital Id</param>
        /// <param name="isUnassociated">Is remove from hospital</param>
        /// <returns></returns>
        Task<Response<HospitalMedicalStaffModel>> AssignToHospitalAsync(int id, int hospitalId, bool isUnassociated);
        /// <summary>
        /// Assign/UnAssign staff to hospital
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="staffIds"></param>
        /// <param name="isUnassociated"></param>
        /// <returns></returns>
        Task<Response<IList<HospitalMedicalStaffModel>>> AssignToHospitalAsync(int hospitalId, List<int> staffIds, bool isUnassociated);
        /// <summary>
        /// Get all doctor by pagination
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Response<PaginationSet<MedicalStaffModel>>> GetAllDoctorByHospitalAsync(int hospitalId, int pageIndex = 1, int pageSize = Constant.PageSize, MedicalStaffStatus[] status = null, bool includeOperatingHours = false, string clue = "");

        /// <summary>
        /// Finds the nearest doctor reference by hospital async.
        /// </summary>
        /// <returns>The nearest doctor reference by hospital async.</returns>
        /// <param name="longitude">Longitude.</param>
        /// <param name="latitude">Latitude.</param>
        /// <param name="radius">Radius.</param>
        /// <param name="polyClinicIds">Poly clinic identifiers.</param>
        /// <param name="search">Search.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Page size.</param>
        Task<Response<PaginationSet<MedicalStaffModel>>> FindNearestDoctorReferenceByHospitalAsync(double longitude, double latitude, int radius, List<int> polyClinicIds,
                                                                                                   string search = "", int pageIndex = 1, int pageSize = Constant.PageSize);

        /// <summary>
        /// Get staff by Id with detail properties
        /// </summary>
        /// <param name="id">int staffId</param>
        /// <returns></returns>
        Task<Response<MedicalStaffModel>> GetSingleDetailAsync(int id);

        /// <summary>
        /// Get all doctor by pagination
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Response<PaginationSet<MedicalStaffModel>>> GetActiveDoctorByHospitalAsync(int hospitalId, int pageIndex = 1, int pageSize = Constant.PageSize, bool includeOperatingHours = false, string clue = "");

        Task<Response<PaginationSet<MedicalStaffModel>>> GetAllDoctorByClueAsync(int pageIndex = 1, int pageSize = Constant.PageSize, string clue = "");

        Task<Response<IList<MedicalStaffSpecialistMapModel>>> AssignSpecialistToStaffAsync(int id, IList<MedicalStaffSpecialistMapModel> specialist);

        /// <summary>
        /// Get all staff sort by distance
        /// </summary>
        /// <param name="hospitalId">Current hospitalId</param>
        /// <param name="radius">radius</param>
        /// <param name="search">search keyword</param>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Response<PaginationSet<MedicalStaffModel>>> GetAvailableStaffSortByDistanceAsync(int hospitalId, int radius,
            MedicalStaffType[] types, MedicalStaffStatus[] status,
            string search = "", double longitude = 0, double latitude = 0, int pageIndex = 1, int pageSize = Constant.PageSize);

        Task<Response<MedicalStaffModel>> ChangeStaffStatusAsync(int id, MedicalStaffStatus status);

        Task<Response<MedicalStaffImageModel>> UploadStaffImageCoverAsync(int id, IFormFile file);
        Task<Response<IList<MedicalStaffImageModel>>> UploadStaffImageAsync(int id, IList<IFormFile> files, bool isPrimary);

        Task<Response<HospitalMedicalStaffModel>> GetStaffOperatingHoursAsync(int hospitalId, int staffId);

        Task<Response<IList<HospitalMedicalStaffModel>>> GetStaffOperatingHoursByDateAsync(int staffId, int dayOfWeek);
    }
}
