using MediCore.Data.Infrastructure;
using MediCore.Service.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Api.InputParam
{
    /// <summary>
    /// User appointment
    /// </summary>
    public class UserAppointmentViewModel
    {
        public int Id { get; set; }
        public AppointmentStatus Status { get; set; }
        [Required(ErrorMessage = "Required")]
        public DateTime AppointmentDate { get; set; }
        [Required(ErrorMessage = "Required")]
        public int HospitalId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int MedicalStaffId { get; set; }

        public List<UserPatientAppointmentModel> PatientProblems { get; set; }
    };

    public class UserPatientAppointmentModel
    {
        [Required(ErrorMessage = "Required")]

        public int PatientId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Problems { get; set; }
    }
}
