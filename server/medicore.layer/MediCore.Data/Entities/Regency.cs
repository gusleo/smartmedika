using dna.core.data.Abstract;
using dna.core.data.Infrastructure;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class Regency : IEntityBase
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int RegionId { get; set; }

        [Required]
        public Status Status { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey("RegionId")]
        public virtual Region Region { get; set; }
    }
}
