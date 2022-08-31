using dna.core.data.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Data.Entities
{
    public class HospitalAppointmentRating : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }
        public int HospitalAppointmentId { get; set; }

        public int Rating { get; set; }

        [MaxLength(300)]
        public string Testimoni { get; set; }

        [ForeignKey("HospitalAppointmentId")]
        public virtual HospitalAppointment HospitalAppointment { get; set; }
    }
}
