using dna.core.service;
using dna.core.service.Mapping;
using MediCore.Data.Entities;
using MediCore.Service.Model;

namespace MediCore.Service
{
    public class DomainToViewModelMappingProfile : DnaDomainToViewModelMapping
    {
        public DomainToViewModelMappingProfile() : base()
        {
            
            /**
             * MaxDepth used to ignore StackOverflowExeption
             * 
             **/
            

            CreateMap<MedicalStaffSpecialist, MedicalStaffSpecialistModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<PolyClinic, PolyClinicModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<PolyClinicToHospitalMap, PolyClinicToHospitalMapModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<Hospital, HospitalModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<Country, CountryModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<Regency, RegencyModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<Region, RegionModel>().MaxDepth(1).IgnoreAllNonExisting(this);           
            CreateMap<UTCTimeBase, UTCTimeBaseModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<MedicalStaff, MedicalStaffModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<MedicalStaffSpecialistMap, MedicalStaffSpecialistMapModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<HospitalMedicalStaff, HospitalMedicalStaffModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<HospitalAppointment, HospitalAppointmentModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<HospitalAppointmentDetail, HospitalAppointmentDetailModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<Patient, PatientModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<HospitalStaffOperatingHours, HospitalOperatingHoursModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<HospitalImage, HospitalImageModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<MedicalStaffImage, MedicalStaffImageModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<MedicalStaffFavorite, MedicalStaffFavoriteModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<UserDetailMediCore, UserDetailMediCoreModel>().MaxDepth(1).IgnoreAllNonExisting(this);
            CreateMap<Notification, NotificationModel>().MaxDepth(1).IgnoreAllNonExisting(this);
        }
    }
}
