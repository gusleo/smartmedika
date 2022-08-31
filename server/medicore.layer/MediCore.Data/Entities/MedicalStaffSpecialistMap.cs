using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class MedicalStaffSpecialistMap : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }
        public int MedicalStaffId { get; set; }
        public int MedicalStaffSpecialistId { get; set; }

        [ForeignKey("MedicalStaffId")]
        public virtual MedicalStaff MedicalStaff { get; set; }
        [ForeignKey("MedicalStaffSpecialistId")]
        public virtual MedicalStaffSpecialist MedicalStaffSpecialist { get; set; }
    }
}
