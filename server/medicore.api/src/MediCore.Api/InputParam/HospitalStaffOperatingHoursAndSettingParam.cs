using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.InputParam
{
    /// <summary>
    /// Custom model for hospital staff
    /// </summary>
    public class HospitalStaffOperatingHoursAndSettingParam
    {
        /// <summary>
        /// Estimate time when handle patient
        /// </summary>
        public int EstimateTimeForPatient { get; set; }

        /// <summary>
        /// Staff operating hours
        /// </summary>
        public IList<HospitalStaffOperatingHoursModel> operatingHours { get; set; }
    }
}
