using Dna.Core.Base.Data;
using MediCore.Lamda.Data.Entitties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System;
using System.Linq;

namespace MediCore.Lamda.Data
{
    public class MediCoreContext : DbContext, IMediCoreContext
    {
        public DbSet<MedicalStaff> MedicalStaff { get; set; }
        public DbSet<HospitalAppointmentRating> HospitalAppointmentRating { get; set; }
        public DbSet<HospitalAppointment> HospitalAppointment { get; set; }
        public DbSet<Hospital> Hospital { get; set; }
        public MediCoreContext(DbContextOptions<MediCoreContext> options) : base(options)
        {
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
        }
        public void Updated<TEntity>(TEntity entity) where TEntity : class
        {
            EntityEntry dbEntityEntry = this.Entry<TEntity>(entity);
            if ( dbEntityEntry.State == EntityState.Detached )
            {
                this.Set<TEntity>().Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }
    }
}
