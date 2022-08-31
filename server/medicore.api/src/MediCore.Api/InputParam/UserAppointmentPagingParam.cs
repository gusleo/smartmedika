using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.InputParam
{
    /// <summary>
    /// Get User Appointment
    /// </summary>
    public class UserAppointmentPagingParam
    {
        /// <summary>
        /// Page Index
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }
      
        /// <summary>
        /// Filter allow null
        /// </summary>
        public AppointmentStatus[] Filter { get; set; }
    }
}
