using dna.core.data.Abstract;
using dna.core.data.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Data.Entities
{

    public class Country : IEntityBase
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(5)]
        public string Code { get; set; }

        [Required]
        public Status Status { get; set; }

        public virtual ICollection<Region> Regions { get; set; }
    }
}
