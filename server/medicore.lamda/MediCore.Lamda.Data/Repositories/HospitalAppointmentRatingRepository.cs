using Dna.Core.Base.Data;
using MediCore.Lamda.Data.Entitties;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MediCore.Lamda.Data.Repositories
{
    public class HospitalAppointmentRatingRepository : EntityReadWriteBaseRepository<HospitalAppointmentRating>, IHospitalAppointmentRatingRepository
    {
        public IMediCoreContext Context
        {
            get { return _context as IMediCoreContext; }
        }
        public HospitalAppointmentRatingRepository(IMediCoreContext context) : base(context)
        {
        }

        public async Task<IList<int>> GetRatingByMedicalStaffAsync(int medicalStaffId)
        {
            var result = (from rat in Context.HospitalAppointmentRating
                          join apt in Context.HospitalAppointment on rat.HospitalAppointmentId equals apt.Id
                          where apt.MedicalStaffId == medicalStaffId
                          select rat.Rating);
            return await result.ToListAsync();
        }

        public async Task<IList<int>> GetRatingByHospitalAsync(int hospitalId)
        {
            var result = (from rat in Context.HospitalAppointmentRating
                          join apt in Context.HospitalAppointment on rat.HospitalAppointmentId equals apt.Id
                          where apt.HospitalId == hospitalId
                          select rat.Rating);
            return await result.ToListAsync();
        }
    }
    public interface IHospitalAppointmentRatingRepository : IReadBaseRepository<HospitalAppointmentRating>, IWriteBaseRepository<HospitalAppointmentRating>
    {
        Task<IList<int>> GetRatingByMedicalStaffAsync(int medicalStaffId);
        Task<IList<int>> GetRatingByHospitalAsync(int medicalStaffId);
    }
}
