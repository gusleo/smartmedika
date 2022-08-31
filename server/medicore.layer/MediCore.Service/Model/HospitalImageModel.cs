using dna.core.service.Models;
using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalImageModel : IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int HospitalId { get; set; }

        [Required(ErrorMessage = "Required")]
        public int ImageId { get; set; }

        
        public virtual HospitalModel Hospital { get; set; }

        public virtual ImageModel Image { get; set; }
    }
}
