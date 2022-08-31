using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class HomeCareAppointmentDetail : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }
        
        [Required]
        public int HomeCareAppointmentId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [MaxLength(255)]
        public string Problem { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        [ForeignKey("HomeCareAppointmentId")]
        public virtual HomeCareAppointment HomeCareAppointment { get; set; }
    }
}
