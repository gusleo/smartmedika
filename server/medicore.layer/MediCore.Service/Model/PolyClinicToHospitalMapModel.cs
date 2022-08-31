using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class PolyClinicToHospitalMapModel : IModelBase
    {
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        public int PolyClinicId { get; set; }
       
        public virtual PolyClinicModel PolyClinic { get; set; }
    }
}
