using dna.core.data.Infrastructure;
using dna.core.data.Repositories.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Repositories.Abstract
{
    public interface ICountryRepository : IReadBaseRepository<Country>, IWriteBaseRepository<Country>
    {
        Task<PaginationEntity<Country>> GetCountryByStatusAsync(Status[] status, int pageIndex, int pageSize);
    }
    public interface INotificationRepository : IReadBaseRepository<Notification>, IWriteBaseRepository<Notification>
    {
     
    }

    public interface IHospitalStaffOperatingHoursRepository : IReadBaseRepository<HospitalStaffOperatingHours>, IWriteBaseRepository<HospitalStaffOperatingHours>
    {
        /// <summary>
        /// Get staff operating hours for each hospital
        /// </summary>
        /// <param name="staffId">int</param>
        /// <param name="dayOfWeek">int</param>
        /// <returns></returns>
        Task<List<HospitalMedicalStaff>> GetStaffOperatingHoursByDateAsync(int staffId, int dayOfWeek);
    }
    public interface IHomeCareAppointmentRepository : IReadBaseRepository<HomeCareAppointment>, IWriteBaseRepository<HomeCareAppointment>
    {

    }

    public interface IHomeCareAppointmentDetailRepository : IReadBaseRepository<HomeCareAppointmentDetail>, IWriteBaseRepository<HomeCareAppointmentDetail>
    {

    }

    public interface IHospitalAppointmentDetailRepository : IWriteBaseRepository<HospitalAppointmentDetail>, IReadBaseRepository<HospitalAppointmentDetail>
    {

    }

    public interface IHospitalImageRepository : IWriteBaseRepository<HospitalImage>, IReadBaseRepository<HospitalImage>
    {

    }

    public interface IHospitalMedicalStaffRepository : IWriteBaseRepository<HospitalMedicalStaff>, IReadBaseRepository<HospitalMedicalStaff>
    {

    }

    public interface IMedicalStaffFavoriteRepository : IWriteBaseRepository<MedicalStaffFavorite>, IReadBaseRepository<MedicalStaffFavorite>
    {
        Task<PaginationEntity<MedicalStaffFavorite>> GetAllByUserAsync(int userId, int pageIndex, int pageSize);

    }

    public interface IHospitalMetadataRepository : IWriteBaseRepository<HospitalMetadata>, IReadBaseRepository<HospitalMetadata>
    {

    }

    public interface IMedicalStaffSpecialistMapRepository : IReadBaseRepository<MedicalStaffSpecialistMap>, IWriteBaseRepository<MedicalStaffSpecialistMap>
    {

    }
    public interface IMedicalStaffSpecialistRepository : IReadBaseRepository<MedicalStaffSpecialist>, IWriteBaseRepository<MedicalStaffSpecialist>
    {

    }
    public interface IMedicalStaffImageRepository : IWriteBaseRepository<MedicalStaffImage>, IReadBaseRepository<MedicalStaffImage>
    {

    }
    public interface IPatientRepository : IReadBaseRepository<Patient>, IWriteBaseRepository<Patient>
    {
        bool AllowedUserToEdit(int id, int userId);
    }
    public interface IPolyClinicRepository : IWriteBaseRepository<PolyClinic>, IReadBaseRepository<PolyClinic>
    {

    }

    public interface IPolyClinicSpesialistMapRepository : IReadBaseRepository<PolyClinicSpesialistMap>, IWriteBaseRepository<PolyClinicSpesialistMap>
    {

    }

    public interface IPolyClinicToHospitalMapRepository : IWriteBaseRepository<PolyClinicToHospitalMap>, IReadBaseRepository<PolyClinicToHospitalMap> {

    }

   

    public interface IRegionRepository : IReadBaseRepository<Region>, IWriteBaseRepository<Region>
    {

    }
    public interface IUserDetailRepository : IReadBaseRepository<UserDetailMediCore>, IWriteBaseRepository<UserDetailMediCore>
    {
        Task<PaginationEntity<UserDetailMediCore>> GetAllWithDetailAsync(int pageIndex, int pageSize, string clue);
        Task<UserDetailMediCore> GetSingleDetail(int userId);
    }
    public interface IUTCTimeBaseRepository : IReadBaseRepository<UTCTimeBase>, IWriteBaseRepository<UTCTimeBase>
    {

    }

    public interface IHospitalAppointmentRatingRepository : IReadBaseRepository<HospitalAppointmentRating>, IWriteBaseRepository<HospitalAppointmentRating>
    {
        Task<IList<HospitalAppointmentRating>> GetTotalHospitalRatingAsync(int hospitalId);
        Task<PaginationEntity<HospitalAppointmentRating>> GetHospitalRatingAsync(int hospitalId, int pageIndex, int pageSize);
        Task<IList<HospitalAppointmentRating>> GetTotalStaffRatingAsync(int staffId);
        Task<PaginationEntity<HospitalAppointmentRating>> GetStaffRatingAsync(int staffId, int pageIndex, int pageSize);
        Task<PaginationEntity<HospitalAppointmentRating>> GetUserRatingAsync(int userId, int pageIndex, int pageSize);
    }
}
