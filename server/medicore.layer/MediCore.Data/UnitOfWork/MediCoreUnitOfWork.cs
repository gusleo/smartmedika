using System;
using dna.core.data.UnitOfWork;
using MediCore.Data.Repositories;
using MediCore.Data.Repositories.Abstract;

namespace MediCore.Data.UnitOfWork
{
    public class MediCoreUnitOfWork : DNAUnitOfWork, IMediCoreUnitOfWork
    {
        protected new IUserDetailRepository _userDetailRepo;
        private IHospitalRepository _hospitalRepo;
        private IHospitalAppointmentDetailRepository _hospitalAppointmentDetailRepo;
        private IHospitalAppointmentRepository _hospitalAppointmentRepo;
        private IHospitalMedicalStaffRepository _hospitalMedicalStaffRepo;
        private IHospitalMetadataRepository _hospitalMetadataRepo;
        private IHospitalOperatorRepository _hospitalOperatorRepo;
        private ICountryRepository _countryRepo;
        private IHomeCareAppointmentRepository _homeCareAppointmentRepo;
        private IHomeCareAppointmentDetailRepository _homeCareAppointmentDetailRepo;
        private IMedicalStaffRepository _medicalStaffRepo;
        private IMedicalStaffSpecialistMapRepository _medicalStaffSpecialistMapRepo;
        private IMedicalStaffSpecialistRepository _medicalStaffSpecialistRepo;
        private IPatientRepository _patientRepo;
        private IRegencyRepository _regencyRepo;
        private IRegionRepository _regionRepo;
        private IUTCTimeBaseRepository _utcTimeRepo;
        private IPolyClinicRepository _polyClinicRepo;
        private IPolyClinicSpesialistMapRepository _polyClinicSpecialistMapRepo;
        private IPolyClinicToHospitalMapRepository _polyClinicToHospitalMapRepo;
        private IHospitalOperatingHoursRepository _hospitalOperatingHoursRepo;
        private IHospitalImageRepository _hospitalImageRepo;
        private IHospitalStaffOperatingHoursRepository _hospitalStaffOperatingHoursRepo;
        private IMedicalStaffImageRepository _medicalStaffImageRepo;
        private IHospitalAppointmentRatingRepository _ratingRepo;
        private IMedicalStaffFavoriteRepository _medicalStaffFavoriteRepo;
        private INotificationRepository _notificationRepo;


        public IMediCoreContext _context;
        public MediCoreUnitOfWork(IMediCoreContext context) : base(context)
        {
            _context = context;
        }

        public new IUserDetailRepository UserDetailRepository
        {
            get
            {
                if ( _userDetailRepo == null )
                {
                    _userDetailRepo = new UserDetailRepository(_context);
                }
                return _userDetailRepo;
            }
        }

        public IHospitalRepository HospitalRepository
        {
            get
            {
                if(_hospitalRepo == null )
                {
                    _hospitalRepo = new HospitalRepository(_context);
                }
                return _hospitalRepo;
            }
        }

        public IHospitalAppointmentDetailRepository HospitalAppointmentDetailRepository
        {
            get
            {
                if ( _hospitalAppointmentDetailRepo == null )
                {
                    _hospitalAppointmentDetailRepo = new HospitalAppointmentDetailRepository(_context);
                }
                return _hospitalAppointmentDetailRepo;
            }
        }

        public IHospitalAppointmentRepository HospitalAppointmentRepository
        {
            get
            {
                if ( _hospitalAppointmentRepo == null )
                {
                    _hospitalAppointmentRepo = new HospitalAppointmentRepository(_context);
                }
                return _hospitalAppointmentRepo;
            }
        }

        public IHospitalMetadataRepository HospitalMetadataRepository
        {
            get
            {
                if ( _hospitalMetadataRepo == null )
                {
                    _hospitalMetadataRepo = new HospitalMetadataRepository(_context);
                }
                return _hospitalMetadataRepo;
            }
        }

        public IHospitalMedicalStaffRepository HospitalMedicalStaffRepository
        {
            get
            {
                if ( _hospitalMedicalStaffRepo == null )
                {
                    _hospitalMedicalStaffRepo = new HospitalMedicalStaffRepository(_context);
                }
                return _hospitalMedicalStaffRepo;
            }
        }

        public IMedicalStaffFavoriteRepository MedicalStaffFavoriteRepository
        {
            get
            {
                if( _medicalStaffFavoriteRepo == null )
                {
                    _medicalStaffFavoriteRepo = new MedicalStaffFavoriteRepository(_context);
                }
                return _medicalStaffFavoriteRepo;
            }
        }

        public IHospitalOperatorRepository HospitalOperatorRepository
        {
            get
            {
                if ( _hospitalOperatorRepo == null )
                {
                    _hospitalOperatorRepo = new HospitalOperatorRepository(_context);
                }
                return _hospitalOperatorRepo;
            }
        }

        public ICountryRepository CountryRepository
        {
            get
            {
                if ( _countryRepo == null )
                {
                    _countryRepo = new CountryRepository(_context);
                }
                return _countryRepo;
            }
        }

        public IHomeCareAppointmentDetailRepository HomeCareAppointmentDetailRepository
        {
            get
            {
                if ( _homeCareAppointmentDetailRepo == null )
                {
                    _homeCareAppointmentDetailRepo = new HomeCareAppointmentDetailRepository(_context);
                }
                return _homeCareAppointmentDetailRepo;
            }
        }

        public IHomeCareAppointmentRepository HomeCareAppointmentRepository
        {
            get
            {
                if ( _homeCareAppointmentRepo == null )
                {
                    _homeCareAppointmentRepo = new HomeCareAppointmentRepository(_context);
                }
                return _homeCareAppointmentRepo;
            }
        }

        public IMedicalStaffRepository MedicalStaffRepository
        {
            get
            {
                if ( _medicalStaffRepo == null )
                {
                    _medicalStaffRepo = new MedicalStaffRepository(_context);
                }
                return _medicalStaffRepo;
            }
        }

        public IMedicalStaffSpecialistMapRepository MedicalStaffSpecialistMapRepository
        {
            get
            {
                if ( _medicalStaffSpecialistMapRepo == null )
                {
                    _medicalStaffSpecialistMapRepo = new MedicalStaffSpecialistMapRepository(_context);
                }
                return _medicalStaffSpecialistMapRepo;
            }
        }

        public IMedicalStaffSpecialistRepository MedicalStaffSpecialistRepository
        {
            get
            {
                if ( _medicalStaffSpecialistRepo == null )
                {
                    _medicalStaffSpecialistRepo = new MedicalStaffSpecialistRepository(_context);
                }
                return _medicalStaffSpecialistRepo;
            }
        }

        public IPatientRepository PatientRepository
        {
            get
            {
                if ( _patientRepo == null )
                {
                    _patientRepo = new PatientRepository(_context);
                }
                return _patientRepo;
            }
        }

        public IRegencyRepository RegencyRepository
        {
            get
            {
                if ( _regencyRepo == null )
                {
                    _regencyRepo = new RegencyRepository(_context);
                }
                return _regencyRepo;
            }
        }

        public IRegionRepository RegionRepository
        {
            get
            {
                if ( _regionRepo == null )
                {
                    _regionRepo = new RegionRepository(_context);
                }
                return _regionRepo;
            }
        }

        public IUTCTimeBaseRepository UTCTimeBaseRepository
        {
            get
            {
                if(_utcTimeRepo == null)
                {
                    _utcTimeRepo = new UTCTimeBaseRepository(_context);
                }
                return _utcTimeRepo;
            }
        }
        public IPolyClinicRepository PolyClinicRepository{
            get
            {
                if(_polyClinicRepo == null )
                {
                    _polyClinicRepo = new PolyClinicRepository(_context);
                }
                return _polyClinicRepo;
            }
        }

        public IPolyClinicSpesialistMapRepository PolyClinicSpecialistMapRepository
        {
            get
            {
                if(_polyClinicSpecialistMapRepo == null )
                {
                    _polyClinicSpecialistMapRepo = new PolyClinicSpesialistMapRepository(_context);
                }
                return _polyClinicSpecialistMapRepo;
            }
        }

        public IPolyClinicToHospitalMapRepository PolyClinicToHospitalMapRepository
        {
            get
            {
                if( _polyClinicToHospitalMapRepo == null )
                {
                    _polyClinicToHospitalMapRepo = new PolyClinicToHospitalMapRepository(_context);
                }
                return _polyClinicToHospitalMapRepo;
            }
        }

        public IHospitalOperatingHoursRepository HospitalOperatingHours
        {
            get
            {
               if(_hospitalOperatingHoursRepo == null )
                {
                    _hospitalOperatingHoursRepo = new HospitalOperatingHoursRepository(_context);
                }
                return _hospitalOperatingHoursRepo;
            }
        }

        public IHospitalImageRepository HospitalImageRepository
        {
            get
            {
                if(_hospitalImageRepo == null )
                {
                    _hospitalImageRepo = new HospitalImageRepository(_context);
                }
                return _hospitalImageRepo;
            }
        }

        

        

        public IHospitalStaffOperatingHoursRepository HospitalStaffOperatingHoursRepository
        {
            get
            {
                if(_hospitalStaffOperatingHoursRepo == null )
                {
                    _hospitalStaffOperatingHoursRepo = new HospitalStaffOperatingHoursRepository(_context);
                }
                return _hospitalStaffOperatingHoursRepo;
            }
        }

        public IMedicalStaffImageRepository MedicalStaffImageRepository
        {
            get
            {
                if ( _medicalStaffImageRepo == null )
                {
                    _medicalStaffImageRepo = new MedicalStaffImageRepository(_context);
                }
                return _medicalStaffImageRepo;
            }
        }

        public IHospitalAppointmentRatingRepository HospitalAppointmentRatingRepository
        {
            get
            {
                if(_ratingRepo == null )
                {
                    _ratingRepo = new HospitalAppointmentRatingRepository(_context);
                }
                return _ratingRepo;
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                if ( _notificationRepo == null )
                {
                    _notificationRepo = new NotificationRepository(_context);
                }
                return _notificationRepo;
            }
        }
    }
}
