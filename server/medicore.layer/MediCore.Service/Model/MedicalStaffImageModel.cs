using dna.core.service.Models;
using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class MedicalStaffImageModel : IModelBase
    {
        public int Id { get; set; }

        [Required]
        public int MedicalStaffId { get; set; }

        [Required]
        public int ImageId { get; set; }

      
        public MedicalStaffModel MedicalStaff { get; set; }

        
        public ImageModel Image { get; set; }
    }
}
