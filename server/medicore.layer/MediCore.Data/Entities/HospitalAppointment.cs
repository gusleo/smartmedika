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
   /// <summary>
   /// Entity of hospital appointment
   /// </summary>
   /// <remarks>
   /// User can make appointment from application (filled UserId)
   /// or user can make by phone. If registered by phone field of PatientName and PhoneNumber 
   /// must be filled.
   /// </remarks>
    public class HospitalAppointment : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        public int MedicalStaffId { get; set; }

        // date to start appointment booking request
        [Required]
        public DateTime AppointmentDate { get; set; }


        // date to start appointment 
        public DateTime? AppointmentStarted { get; set; }

        // date to end appointment 
        public DateTime? AppointmentFinished { get; set; }

        [Required]
        public AppointmentStatus AppointmentStatus { get; set; }

        [Required]
        public int QueueNumber { get; set; }


        //if registered user then he/she is user
        public int? UserId { get; set; }

        //if not registered user then he/she use PhoneNumber   
        [MaxLength(50)]
        public string PhoneNumber { get; set; }

        //if not registered user then he/she use PatientName   
        [MaxLength(255)]
        public string PatientName { get; set; }

        //if not registered user fill on PatientProblems
        //if registered user, fill on HospitalAppointmentDetail
        [MaxLength(300)]
        public string PatientProblems { get; set; }

        //if user cancelled, fill the reason
        //cancelled status is from AppointmentStatus
        [MaxLength(300)]
        public string CancelledReason { get; set; }
        

        //Foreign

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

        [ForeignKey("MedicalStaffId")]
        public virtual MedicalStaff MedicalStaff { get; set; }

        public virtual ICollection<HospitalAppointmentDetail> AppointmentDetails { get; set; }

        [NotMapped]
        public int CurrentQueueNumber { get; set; }

        
    }
}
