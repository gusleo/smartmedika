using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalAppointmentRatingModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }
        public int HospitalAppointmentId { get; set; }

        public int Rating { get; set; }

        [StringLength(300, ErrorMessage = "StringLength")]
        public string Testimoni { get; set; }
      
        public virtual HospitalAppointmentModel HospitalAppointment { get; set; }
    }
}
