using dna.core.data.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
   
    public class MedicalStaffSpecialist : IEntityBase
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Alias { get; set; }
       
        public Nullable<int> PolyClinicId { get; set; }

        public string Description { get; set; }

        [Required]
        public MedicalStaffType StaffType { get; set; }

        [ForeignKey("PolyClinicId")]
        public virtual PolyClinic PolyClinic { get; set; }
    }
}
