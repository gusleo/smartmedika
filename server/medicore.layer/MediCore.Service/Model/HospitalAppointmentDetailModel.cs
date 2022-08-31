using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalAppointmentDetailModel : IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int HospitalAppointmentId { get; set; }

        [Required(ErrorMessage = "Required")]
        public int PatientId { get; set; }

        [StringLength(100, ErrorMessage = "StringLength")]
        public string Problem { get; set; }

        
        public virtual HospitalModel Hospital { get; set; }
       
        public virtual PatientModel Patient { get; set; }

        public virtual HospitalAppointmentModel HospitalAppointment { get; set; }
    }
}
