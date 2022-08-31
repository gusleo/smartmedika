using dna.core.data.Abstract;
using dna.core.data.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Data.Entities
{
    public class UTCTimeBase : IEntityBase
    {
        public int Id { get; set; }
        [Required]
        public int CountryId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(10)]
        public string Code { get; set; }

        //UTC is +8, -8, etc
        [Required]
        public int UTC { get; set; }
        [Required]
        public Status Status { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

    }
}
