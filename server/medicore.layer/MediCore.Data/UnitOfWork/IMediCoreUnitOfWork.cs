using dna.core.data.UnitOfWork;
using MediCore.Data.Repositories;
using MediCore.Data.Repositories.Abstract;

namespace MediCore.Data.UnitOfWork
{
    public interface IMediCoreUnitOfWork : IDNAUnitOfWork
    {
        new IUserDetailRepository UserDetailRepository { get; }
        IHospitalRepository HospitalRepository { get; }
        IHospitalAppointmentDetailRepository HospitalAppointmentDetailRepository { get; }
        IHospitalAppointmentRepository HospitalAppointmentRepository { get; }
        IHospitalMetadataRepository HospitalMetadataRepository { get; }
        IHospitalMedicalStaffRepository HospitalMedicalStaffRepository { get; }
        IHospitalOperatorRepository HospitalOperatorRepository { get; }
        ICountryRepository CountryRepository { get; }
        IHomeCareAppointmentDetailRepository HomeCareAppointmentDetailRepository { get; }
        IHomeCareAppointmentRepository HomeCareAppointmentRepository { get; }
        IMedicalStaffRepository MedicalStaffRepository { get; }
        IMedicalStaffSpecialistMapRepository MedicalStaffSpecialistMapRepository { get; }
        IMedicalStaffFavoriteRepository MedicalStaffFavoriteRepository { get; }
        IMedicalStaffSpecialistRepository MedicalStaffSpecialistRepository { get; }
        IPatientRepository PatientRepository { get; }
        IRegencyRepository RegencyRepository { get; }
        IRegionRepository RegionRepository { get; }
        IUTCTimeBaseRepository UTCTimeBaseRepository { get; }
        IPolyClinicRepository PolyClinicRepository { get; }
        IPolyClinicSpesialistMapRepository PolyClinicSpecialistMapRepository { get; }
        IPolyClinicToHospitalMapRepository PolyClinicToHospitalMapRepository { get; }
        IHospitalOperatingHoursRepository HospitalOperatingHours { get; }
        IHospitalImageRepository HospitalImageRepository { get; }
        IHospitalStaffOperatingHoursRepository HospitalStaffOperatingHoursRepository { get; }
        IMedicalStaffImageRepository MedicalStaffImageRepository { get; }
        IHospitalAppointmentRatingRepository HospitalAppointmentRatingRepository { get; }

        INotificationRepository NotificationRepository { get; }

    }
}
