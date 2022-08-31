using System.Collections.Generic;

namespace MediCore.Api.InputParam
{
    public class AssignStaffToHospitalParam
    {
        public int HospitalId { get; set; }
        public List<int> StaffIds { get; set; }
        public bool IsDeleteFromHospital { get; set; }
    }
}
