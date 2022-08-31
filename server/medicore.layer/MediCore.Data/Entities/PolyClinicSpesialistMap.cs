using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class PolyClinicSpesialistMap : IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int MedicalStaffSpecialistId { get; set; }

        [Required]
        public int PolyClinicId { get; set; }

        [ForeignKey("MedicalStaffSpecialistId")]
        public virtual MedicalStaffSpecialist Specialist { get; set; }

        [ForeignKey("PolyClinicId")]
        public virtual PolyClinic PolyClinic { get; set; }
    }
}
