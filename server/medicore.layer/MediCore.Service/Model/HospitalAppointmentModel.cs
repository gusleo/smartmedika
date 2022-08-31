using dna.core.service.Models;
using dna.core.service.Models.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalAppointmentModel : WriteHistoryBaseModel, IModelBase
    {
        

        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int HospitalId { get; set; }

        [Required(ErrorMessage = "Required")]
        public int MedicalStaffId { get; set; }

        [Required(ErrorMessage = "Required")]
        public DateTime AppointmentDate { get; set; }

        public DateTime? AppointmentStarted { get; set; }
        //fill when appointment finished
        public DateTime? AppointmentFinished { get; set; }

        [Required(ErrorMessage = "Required")]
        public AppointmentStatus AppointmentStatus { get; set; }

        [Required(ErrorMessage = "Required")]
        public int QueueNumber { get; set; }


        //if appointment create the by the logged user then fill it
        public int? UserId { get; set; }

        //if not registered user then he/she use PhoneNumber        

        [StringLength(50, ErrorMessage = "StringLength")]
        public string PhoneNumber { get; set; }

        //if not registered user then he/she use PatientName   
        [StringLength(255, ErrorMessage = "StringLength")]
        public string PatientName { get; set; }


        //if not registered user fill on PatientProblems
        //if registered user, fill on HospitalAppointmentDetail
        [StringLength(300, ErrorMessage = "StringLength")]
        public string PatientProblems { get; set; }

        //if user cancelled, fill the reason
        //cancelled status is from AppointmentStatus
        [StringLength(300, ErrorMessage = "StringLength")]
        public string CancelledReason { get; set; }

        public virtual UserModel User { get; set; }

       
        public virtual HospitalModel Hospital { get; set; }

       
        public virtual MedicalStaffModel MedicalStaff { get; set; }

        //token from device for sending FCM, not saved in DB
        public string DeviceId { get; set; }

        public virtual ICollection<HospitalAppointmentDetailModel> AppointmentDetails { get; set; }

        // not mapped
        public int CurrentQueueNumber { get; set; }
    }
}
