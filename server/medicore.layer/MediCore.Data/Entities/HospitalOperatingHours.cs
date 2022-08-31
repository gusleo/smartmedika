using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class HospitalOperatingHours : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }

        [Required]
        public int Day { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        [MaxLength(10)]
        public string StartTime { get; set; }

        [Required]
        [MaxLength(10)]
        public string EndTime { get; set; }

        [Required]
        public bool IsClossed { get; set; }

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }
    }
}
