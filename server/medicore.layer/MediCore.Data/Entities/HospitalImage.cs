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
    public class HospitalImage : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        public int ImageId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }

        [ForeignKey("ImageId")]
        public Image Image { get; set; }

    }
}
