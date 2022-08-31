using dna.core.data;
using MediCore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data
{
    public interface IMediCoreContext : IDnaCoreContext
    {
        new DbSet<UserDetailMediCore> UserDetail { get; set; }
        DbSet<Hospital> Hospital { get; set; }
        DbSet<HospitalAppointment> HospitalAppointment { get; set; }
        DbSet<HospitalAppointmentDetail> HospitalAppointmentDetail { get; set; }
        DbSet<HospitalMedicalStaff> HospitalMedicalStaff { get; set; }
        DbSet<HospitalOperator> HospitalOperator { get; set; }
        DbSet<Country> Country { get; set; }
        DbSet<HomeCareAppointment> HomeCareAppointment { get; set; }
        DbSet<HomeCareAppointmentDetail> HomeCareAppointmentDetail { get; set; }
        DbSet<MedicalStaff> MedicalStaff { get; set; }
        DbSet<MedicalStaffSpecialist> MedicalStaffSpecialist { get; set; }
        DbSet<MedicalStaffSpecialistMap> MedicalStaffSpecialistMap { get; set; }
        DbSet<Patient> Patient { get; set; }
        DbSet<PolyClinic> PolyClinic { get; set; }
        DbSet<PolyClinicToHospitalMap> PolyClinicToHospitalMap { get; set; }
        DbSet<PolyClinicSpesialistMap> PolyClinicSpesialistMap { get; set; }
        DbSet<Region> Region { get; set; }
        DbSet<Regency> Regency { get; set; }
        DbSet<UTCTimeBase> UTCTimeBase { get; set; }
        DbSet<HospitalOperatingHours> HospitalOperatingHours { get; set; }
        DbSet<HospitalImage> HospitalImage { get; set; }
        DbSet<HospitalStaffOperatingHours> StaffOperatingHours { get; set; }
        DbSet<MedicalStaffImage> MedicalStaffImage { get; set; }
        DbSet<HospitalAppointmentRating> HospitalAppointmentRating { get; set; }
        DbSet<MedicalStaffFavorite> MedicalStaffFavorite { get; set; }
        DbSet<Notification> Notification { get; set; }
    }
}
