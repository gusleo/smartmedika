using System;
using System.Collections.Generic;
using System.Text;

namespace MediCore.Lamda.Data.Entitties
{
    public enum MedicalStaffType
    {
        Nurse = 0,
        Doctor = 1,
        MidWife = 2,
        Therapist = 3
    }
    public enum AppointmentStatus
    {
        Cancel = 0,
        Finish = 1,
        Pending = 2,
        Process = 3,

    }
    public enum HospitalStatus
    {
        InActive = 0,
        Active = 1,
        Suspended = 2
    }
}
