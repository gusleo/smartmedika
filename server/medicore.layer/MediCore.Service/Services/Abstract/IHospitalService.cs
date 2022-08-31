using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using MediCore.Service.Model.Custom;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public interface IHospitalService : IReadWriteService<HospitalModel, Hospital>
    {

        /// <summary>
        /// Upload hospital image gallery
        /// </summary>
        /// <param name="id">HospitalId</param>
        /// <param name="files">Form File</param>
        /// <returns></returns>
        Task<Response<IList<HospitalImageModel>>> UploadHospitalImageAsync(int id, IList<IFormFile> files, bool isPrimary);

        /// <summary>
        /// Upload image for cover
        /// </summary>
        /// <param name="id">HospitalId</param>
        /// <param name="file">Form File</param>
        /// <returns></returns>
        Task<Response<HospitalImageModel>> UploadHospitalImageCoverAsync(int id, IFormFile file);
        /// <summary>
        /// Get hospital detail
        /// </summary>
        /// <param name="id"><HospitalId</param>
        /// <returns></returns>
        Task<Response<HospitalModel>> GetHospitalDetailAsync(int id);
        /// <summary>
        /// Update hospital metadata
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modelToEdit"></param>
        /// <returns></returns>
        Task<Response<HospitalMetadataModel>> UpdateHospitalMetadataAsync(int id, HospitalMetadataModel modelToEdit);

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
        Task<Response<PaginationSet<HospitalModel>>> FindNearestHospitalAsync(double longitude, double latitude, int radius, List<int> polyClinicIds, int pageIndex, int pageSize, string clue = "");
        /// <summary>
        /// Get all hospital by status filter
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<Response<PaginationSet<HospitalModel>>> FindHospitalByFilterAsync(int pageIndex, int pageSize, int regionId = 0, string clue = "", HospitalStatus[] status = null);
        /// <summary>
        /// Get aktive hospital
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<Response<PaginationSet<HospitalModel>>> GetActiveHospitalAsync(int pageIndex, int pageSize);

        /// <summary>
        /// Change hospital status
        /// </summary>
        /// <param name="id">HospitalId</param>
        /// <param name="status"><see cref="HospitalStatus"/></param>
        /// <returns></returns>
        Task<Response<HospitalModel>> ChangeHospitalStatusAsync(int id, HospitalStatus status);

        /// <summary>
        /// Get hospital assocaited to user
        /// </summary>
        /// <returns></returns>
        Task<Response<IList<HospitalModel>>> GetHospitalAssocatedUserAsync();

        new Task<Response<HospitalModel>> Create(HospitalModel modelToCreate);
        new Task<Response<HospitalModel>> Edit(HospitalModel modelToEdit);

    }
}
