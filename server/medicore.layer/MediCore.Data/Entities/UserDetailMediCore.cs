using dna.core.data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class UserDetailMediCore : UserDetail
    {
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        [MaxLength(255)]
        public string Address { get; set; }

        public int? RegencyId { get; set; }
        public int? PatientId { get; set; }

        [ForeignKey("RegencyId")]
        public virtual Regency Regency { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}
