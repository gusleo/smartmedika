using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Services.Abstract
{
    public interface IHospitalAppointmentService : IReadWriteService<HospitalAppointmentModel, HospitalAppointment>
    {
       
        /// <summary>
        /// Add/edit appointment detail
        /// </summary>
        /// <param name="id">Appointment Id</param>
        /// <param name="appointmentDetails">Appointment Detail <see cref="HomeCareAppointmentDetail"/></param>
        /// <returns></returns>
        Task<Response<HospitalAppointmentDetailModel>> AddEditAppointmentDetailAsync(int id, HospitalAppointmentDetailModel appointmentDetail);
       
        Task<Response<PaginationSet<HospitalAppointmentModel>>> GetHospitalAppointmentAsync(int HospitalId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize = Constant.PageSize, AppointmentStatus[] filter = null);
        Task<Response<PaginationSet<HospitalAppointmentModel>>> GetHospitalAppointmentByDoctorAsync(int HospitalId, int staffId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize = Constant.PageSize, AppointmentStatus[] filter = null);
        Task<Response<PaginationSet<HospitalAppointmentModel>>> GetUserAppointmentAsync(int userId = 0, int pageIndex = 1, int pageSize = Constant.PageSize, AppointmentStatus[] filter = null);
        Task<Response<PaginationSet<HospitalAppointmentModel>>> GetUserAppointmentNotRatedAsync(int userId = 0, int pageIndex = 1, int pageSize = Constant.PageSize, AppointmentStatus[] filter = null);

        /// <summary>
        /// Delete appointment detail
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Response<HospitalAppointmentDetailModel>> DeleteAppointmentDetailAsync(int id);
        /// <summary>
        /// Assign specialist to medical staff
        /// </summary>
        /// <param name="modelToCreate"></param>
        /// <returns></returns>
        Task<Response<HospitalAppointmentModel>> CreateForNonRegisteredUserAsync(HospitalAppointmentModel modelToCreate);

        new Task<Response<HospitalAppointmentModel>> Create(HospitalAppointmentModel modelToCreate);
        new Response<HospitalAppointmentModel> Edit(HospitalAppointmentModel modelToEdit);
        Task<Response<HospitalAppointmentModel>> ProcessAppointment(int id);
        Task<Response<HospitalAppointmentModel>> FinishAppointment(int id);
        Task<Response<HospitalAppointmentModel>> CancelAppointment(int id, string reason);
        Task<Response<HospitalAppointmentModel>> GetEstimateAsync(int id);
        Task<Response<HospitalAppointmentModel>> ChangeAppointmentStatusAsync(int id, AppointmentStatus status);
    }
}
