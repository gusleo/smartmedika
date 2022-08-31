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
    public interface IHospitalAppointmentRepository : IWriteBaseRepository<HospitalAppointment>, IReadBaseRepository<HospitalAppointment>
    {
        Task<int> GetMaxQueueNumberAsync(int hospitalId, int medicalStaffId, DateTime appointmentDate);
        Task<PaginationEntity<HospitalAppointment>> GetUserAppointmentAsync(int userId, AppointmentStatus[] filter, int pageIndex, int pageSize);
        Task<PaginationEntity<HospitalAppointment>> GetUserAppointmentNotRatedAsync(int userId, AppointmentStatus[] filter, int pageIndex, int pageSize);

        /// <summary>
        /// Get Hospital And Staff Appointment
        /// If StaffId equal to zero then query will not filter staff
        /// </summary>
        /// <param name="hospitalId">Hospital Identifier</param>
        /// <param name="staffId">Staff Identifier</param>
        /// <param name="startDate">Date start of appoitment</param>
        /// <param name="endDate">Date end of appoitment</param>
        /// <param name="pageIndex">Page Index</param>
        /// <param name="pageSize">Page Width</param>
        /// <param name="filter">Appoitnment Filter</param>
        /// <returns></returns>
        Task<PaginationEntity<HospitalAppointment>> GetHospitalStaffAppointmentAsync(int hospitalId, int staffId, DateTime startDate, DateTime endDate, int pageIndex, int pageSize, AppointmentStatus[] filter = null);
        Task<HospitalAppointment> GetHospitalAppointmentDetailAsync(int id);
        Task<HospitalAppointment> GetQuequeEstimateAsync(int id);
        int GetHospitalAppointmentUserId(int id);

    }
}
