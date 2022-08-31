using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class HospitalAppointmentDetail : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int HospitalAppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }
        [MaxLength(255)]
        public string Problem { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("HospitalAppointmentId")]
        public virtual HospitalAppointment HospitalAppointment { get; set; }
    }
}
