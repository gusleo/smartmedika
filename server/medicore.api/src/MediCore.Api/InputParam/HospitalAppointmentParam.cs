using MediCore.Data.Infrastructure;
using System;

namespace MediCore.Api.InputParam
{
    /// <summary>
    /// Input parameter for HospitalAppointment
    /// </summary>
    public class HospitalAppointmentParam
    {

        public int HospitalId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int StaffId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        /// <summary>
        /// Filter allow null
        /// </summary>
        public AppointmentStatus[] Filter { get; set; }
    }
}
