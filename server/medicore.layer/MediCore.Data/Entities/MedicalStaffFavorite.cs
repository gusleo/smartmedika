using dna.core.auth.Entity;
using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MediCore.Data.Entities
{
    public class MedicalStaffFavorite  : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MedicalStaffId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("MedicalStaffId")]
        public virtual MedicalStaff MedicalStaff { get; set; }
    }
}
