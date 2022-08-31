using dna.core.auth.Infrastructure;
using dna.core.libs.Sender.ConfigType;
using dna.core.libs.Upload;
using dna.core.libs.Upload.Abstract;
using dna.core.libs.Upload.Config;
using dna.core.libs.Validation;
using dna.core.service;
using MediCore.Data;
using MediCore.Data.UnitOfWork;
using MediCore.Service.Services;
using MediCore.Service.Services.Abstract;
using MediCore.Service.Services.Extend;
using MediCore.Service.Services.Extend.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace MediCore.Api
{
    public partial class Startup
    {
        /// <summary>
        /// Adding dependency injection
        /// </summary>
        /// <param name="services">Service collection <see cref="IServiceCollection"/></param>
        public void DepedencyInjection(IServiceCollection services)
        {

            services.AddScoped<IMediCoreContext>(provider => provider.GetService<MediCoreContext>());
            
            //Azure Blob
            //services.AddTransient<IFileServices, FileServices>();
            //services.Configure<AzureStorageOption>(Configuration.GetSection("AzureStorage"));

            //Messaging         
            services.Configure<SenderConfiguration>(Configuration.GetSection("Sender"));          
           

            //Unit Of Work
            services.AddScoped<IMediCoreUnitOfWork, MediCoreUnitOfWork>();

            //validation
            services.AddScoped<IValidationDictionary, ModelStateWrapper>();

            

            //services from dna.core
            services.AddDnaCoreDependency();
            services.AddDnaAuthentication();

            

            //services from MediCore
            services.AddScoped<IMedicalSpecialistService, MedicalSpecialistService>();
            services.AddScoped<IPolyClinicService, PolyClinicService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IMedicalStaffService, MedicalStaffService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IUTCTimeBaseService, UTCTimeBaseService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IRegencyService, RegencyService>();
            services.AddScoped<IHospitalAppointmentService, HospitalAppointmentService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IHospitalOperatorService, HospitalOperatorService>();
            services.AddScoped<IHospitalAppointmentRatingService, HospitalAppointmentRatingService>();
            services.AddScoped<IUserDetailService, UserDetailService>();
            services.AddScoped<IMedicalStaffFavoriteService, MedicalStaffFavoriteService>();
            services.AddScoped<INotificationService, NotificationServices>();

            //extend
            services.AddScoped<IMenuBuilderService, MenuBuilderService>();

            //Upload destination file
            services.AddTransient<IUploadService, UploadService>();
            services.Configure<ServerConfig>(Configuration.GetSection("ServerStorage"));



        }
    }
}
