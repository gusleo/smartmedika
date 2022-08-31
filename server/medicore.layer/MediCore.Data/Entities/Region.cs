using dna.core.data.Abstract;
using dna.core.data.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Data.Entities
{
    public class Region : IEntityBase
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int UTCId { get; set; }
        [Required]
        public Status Status { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        [ForeignKey("UTCId")]
        public virtual UTCTimeBase UTCTime { get; set; }

        public virtual ICollection<Regency> Regencys { get; set; }
       

    }
}
