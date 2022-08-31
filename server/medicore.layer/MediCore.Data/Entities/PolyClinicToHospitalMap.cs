using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class PolyClinicToHospitalMap : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        public int PolyClinicId { get; set; }

        [ForeignKey("PolyClinicId")]
        public virtual PolyClinic PolyClinic { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }



    }
}
