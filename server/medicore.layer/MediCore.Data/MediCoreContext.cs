using dna.core.data;
using MediCore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System;
using System.Linq;

namespace MediCore.Data
{
    public class MediCoreContext : DnaCoreContext, IMediCoreContext
    {
        public MediCoreContext(DbContextOptions<MediCoreContext> options) : base(ChangeOptionsType<DnaCoreContext>(options))
        {
   
        }

        public static DbContextOptions<T> ChangeOptionsType<T>(DbContextOptions options) where T : DbContext
        {
           
            var sqlExt = options.Extensions.FirstOrDefault(e => e is SqlServerOptionsExtension);
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            if ( sqlExt == null )
                throw (new Exception("Failed to retrieve SQL connection string for base Context"));

            return new DbContextOptionsBuilder<T>()
                       .UseSqlServer(((SqlServerOptionsExtension)sqlExt).ConnectionString,
                        b => b.MigrationsAssembly("MediCore.Api"))
                        .Options;
        }

        public new DbSet<UserDetailMediCore> UserDetail { get; set; }

        public virtual DbSet<Hospital> Hospital { get; set; }
        public virtual DbSet<HospitalAppointment> HospitalAppointment { get; set; }
        public virtual DbSet<HospitalAppointmentDetail> HospitalAppointmentDetail { get; set; }
        public virtual DbSet<HospitalMedicalStaff> HospitalMedicalStaff { get; set; }
        public virtual DbSet<HospitalOperator> HospitalOperator { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public DbSet<HomeCareAppointment> HomeCareAppointment { get; set; }
        public DbSet<HomeCareAppointmentDetail> HomeCareAppointmentDetail { get; set; }
        public DbSet<MedicalStaff> MedicalStaff { get; set; }
        public DbSet<MedicalStaffSpecialist> MedicalStaffSpecialist { get; set; }
        public DbSet<MedicalStaffSpecialistMap> MedicalStaffSpecialistMap { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<PolyClinic> PolyClinic { get; set; }
        public DbSet<PolyClinicToHospitalMap> PolyClinicToHospitalMap { get; set; }
        public DbSet<PolyClinicSpesialistMap> PolyClinicSpesialistMap { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Regency> Regency { get; set; }
        public DbSet<UTCTimeBase> UTCTimeBase { get; set; }       
        public DbSet<HospitalOperatingHours> HospitalOperatingHours { get; set; }
        public DbSet<HospitalImage> HospitalImage { get; set; }
        public DbSet<HospitalStaffOperatingHours> StaffOperatingHours { get; set; }
        public DbSet<MedicalStaffImage> MedicalStaffImage { get; set; }
        public DbSet<HospitalAppointmentRating> HospitalAppointmentRating { get; set; }

        public DbSet<MedicalStaffFavorite> MedicalStaffFavorite { get; set; }
        public DbSet<Notification> Notification { get; set; }
        /// <summary>
        /// By default table behaviour is cascade delete, but we need some table not set as auto delete
        /// Set delete behaviour from cascade delete to cascade restrict
        /// Define manualy which table will cascade delete
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Remove Cascade Delate Bahaviour
            foreach ( var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()) )
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //Define manual which table will cascade delete

            modelBuilder.Entity<HospitalOperatingHours>().HasOne(x => x.Hospital)
                .WithMany(x => x.OperatingHours).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HospitalImage>().HasOne(x => x.Hospital)
                 .WithMany(x => x.Images).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<HospitalImage>().HasOne(x => x.Image)
                 .WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MedicalStaffImage>().HasOne(x => x.MedicalStaff)
                 .WithMany(x => x.Images).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MedicalStaffImage>().HasOne(x => x.Image)
                 .WithOne().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<HospitalStaffOperatingHours>().HasOne(x => x.HospitalStaff)
               .WithMany(x => x.OperatingHours).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PolyClinicToHospitalMap>().HasOne(x => x.Hospital)
                .WithMany(x => x.PolyClinicMaps).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PolyClinicToHospitalMap>().HasOne(x => x.PolyClinic)
                .WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PolyClinicSpesialistMap>().HasOne(x => x.PolyClinic)
                .WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<PolyClinicSpesialistMap>().HasOne(x => x.Specialist)
                .WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MedicalStaffFavorite>().HasOne(x => x.MedicalStaff)
                .WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<MedicalStaffFavorite>().HasOne(x => x.User)
              .WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
