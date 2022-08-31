using dna.core.auth.Entity;
using dna.core.data.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class HomeCareAppointment : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int MedicalStaffId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }


        [Required]
        public AppointmentStatus AppointmentStatus { get; set; }

        [Required]
        public AppointmentCanceled CanceledStatus { get; set; }

        [Required]
        public Double Longitude { get; set; }

        [Required]
        public Double Latitude { get; set; }

        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }

        public int Rating { get; set; }

        [ForeignKey("MedicalStaffId")]
        public virtual MedicalStaff MedicalStaff { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
