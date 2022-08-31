using Dna.Core.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MediCore.Lamda.Data.Entitties
{
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
        
    }
}
