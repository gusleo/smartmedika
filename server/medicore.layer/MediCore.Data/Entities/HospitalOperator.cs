using dna.core.auth.Entity;
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
    /// <summary>
    /// Class of mapping between user and hospital
    /// </summary>
    /// <remarks>
    /// User saved is user have role of Operator and Admin
    /// </remarks>
    public class HospitalOperator : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public HospitalStaffStatus Status { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
