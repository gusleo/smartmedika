using Dna.Core.Base.Data;
using MediCore.Lamda.Data.Entitties;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Lamda.Data
{
    public interface IMediCoreContext : IBaseDbContext
    {
        DbSet<MedicalStaff> MedicalStaff { get; set; }
        DbSet<HospitalAppointmentRating> HospitalAppointmentRating { get; set; }
        DbSet<HospitalAppointment> HospitalAppointment { get; set; }
        DbSet<Hospital> Hospital { get; set; }

    }
}
