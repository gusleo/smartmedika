using dna.core.data.Abstract;
using dna.core.data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class MedicalStaffImage : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int MedicalStaffId { get; set; }

        [Required]
        public int ImageId { get; set; }

        [ForeignKey("MedicalStaffId")]
        public MedicalStaff MedicalStaff { get; set; }

        [ForeignKey("ImageId")]
        public Image Image { get; set; }

    }
}
