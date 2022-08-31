using dna.core.service.Infrastructure;
using dna.core.service.Services.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediCore.Service.Services
{
    public interface IMedicalSpecialistService : IReadWriteService<MedicalStaffSpecialistModel, MedicalStaffSpecialist>
    {
        /// <summary>
        /// Create multiple specialist
        /// </summary>
        /// <param name="listOfData"></param>
        /// <returns></returns>
        Response<IList<MedicalStaffSpecialistModel>> CreateRange(List<MedicalStaffSpecialistModel> listOfData);
    }
    public interface IPolyClinicService : IReadWriteService<PolyClinicModel, PolyClinic>
    {
        /// <summary>
        /// Assign specilist to polyclinic
        /// </summary>
        /// <param name="polyClinicId"></param>
        /// <param name="specialistIds"></param>
        /// <returns></returns>
        Response<PolyClinicModel> AddSpecialistToPolyClinic(int polyClinicId, int[] specialistIds);
        /// <summary>
        /// Create multiple polyclinic
        /// </summary>
        /// <param name="listOfData"></param>
        /// <returns></returns>
        Response<IList<PolyClinicModel>> CreateRange(List<PolyClinicModel> listOfData);
        Task<Response<IList<PolyClinicModel>>> GetByClueAsync(string clue);
    }

    public interface IUserDetailService : IReadWriteService<UserDetailMediCoreModel, UserDetailMediCore> {
        Task<Response<PaginationSet<UserDetailMediCoreModel>>> GetAllWithDetailAsync(int pageIndex, int pageSize, string clue);
        Task<Response<UserDetailMediCoreModel>> GetSingleWithPatientAsync(int id);
        Task<Response<UserDetailMediCoreModel>> EditWithPatientAsync(UserDetailMediCoreModel model);
        Task<Response<UserDetailMediCoreModel>> GetDetailByUserId(int userId);
    }

    public interface INotificationService : IReadWriteService<NotificationModel, Notification>
    {
        Task<Response<PaginationSet<NotificationModel>>> GetNotificationByObjectId(int objectId, int pageIndex, int pageSize);
        Task<Response<IList<NotificationModel>>> GetUserUnReadNotification();
    }



    public interface IHospitalAppointmentRatingService : IReadWriteService<HospitalAppointmentRatingModel, HospitalAppointmentRating>
    {
        Task<Response<IList<HospitalAppointmentRatingModel>>> GetTotalHospitalRatingAsync(int hospitalId);
        Task<Response<PaginationSet<HospitalAppointmentRatingModel>>> GetHospitalRatingAsync(int hospitalId, int pageIndex, int pageSize);
        Task<Response<IList<HospitalAppointmentRatingModel>>> GetTotalStaffRatingAsync(int staffId);
        Task<Response<PaginationSet<HospitalAppointmentRatingModel>>> GetStaffRatingAsync(int staffId, int pageIndex, int pageSize);
        Task<Response<PaginationSet<HospitalAppointmentRatingModel>>> GetUserRatingAsync(int pageIndex, int pageSize);
    }
}