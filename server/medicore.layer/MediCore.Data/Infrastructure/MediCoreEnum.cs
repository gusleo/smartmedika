using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Infrastructure
{
    
    public enum MetaType
    {
        Phones = 0,
        OperatingHours = 1,
        PhotoGallery = 2,
        Email = 3
    }
    public enum AppointmentStatus
    {
        Cancel = 0,
        Finish = 1,
        Pending = 2,
        Process = 3,       
       
    }

    public enum MedicalStaffType
    {
        Nurse = 0,
        Doctor = 1,       
        MidWife = 2,
        Therapist = 3
    }

    public enum AppointmentCanceled
    {
        NotCanceled = 0,
        CanceledByStaff = 1,
        CanceledByUser = 2
    }

    public enum RelationshipStatus
    {
        YourSelf = 0,
        Spouse = 1,
        Parent = 2,
        Child = 3,
        Other = 4

    }

    public enum PatientStatus
    {
        InActive = 0,
        Active = 1,
        Death = 2
        
    }

    public enum HospitalStatus
    {
        InActive = 0,
        Active = 1,
        Suspended = 2
    }

    public enum HospitalStaffStatus
    {
        InActive = 0,
        Active = 1,
        Suspended = 2
    }
    public enum MedicalStaffStatus
    {
        InActive = 0,
        Active = 1,
        Death = 2,
        Suspended = 3,
        Pending = 4
    }
    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}
